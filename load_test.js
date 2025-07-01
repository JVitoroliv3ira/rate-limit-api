import { check, sleep } from 'k6';
import { randomBytes } from 'k6/crypto';
import http from 'k6/http';

export const options = {
  stages: [
    { duration: '10s', target: 20 },
    { duration: '50s', target: 20 },
    { duration: '10s', target: 0 },
  ],
  thresholds: {
    http_req_duration: ['p(95)<400'],
    http_req_failed:   ['rate<0.02'],
  },
};

const BASE      = __ENV.BASE_URL   || 'http://localhost:5000/api/v1';
const EMAIL     = __ENV.TEST_EMAIL || 'load@demo.com';
const PASSWORD  = __ENV.PASSWORD   || 'SuperSecret123!';

export function setup() {
  http.post(`${BASE}/Auth/register`,
    JSON.stringify({ email: EMAIL, password: PASSWORD }),
    { headers: { 'Content-Type': 'application/json' } });

  const login = http.post(`${BASE}/Auth/token`,
    JSON.stringify({ email: EMAIL, password: PASSWORD }),
    { headers: { 'Content-Type': 'application/json' } });

  check(login, { 'login ok': r => r.status === 200 });
  const jwt = login.json().accessToken;

  const name  = `lt-${randomBytes(4, 'hex')}`;
  const create = http.post(`${BASE}/ApiKey`,
    JSON.stringify({ name }),
    {
      headers: {
        'Content-Type': 'application/json',
        Authorization: `Bearer ${jwt}`,
      },
    });

  check(create, { 'key created 201': r => r.status === 201 });

  const apiKey = create.json().key;

  check({ apiKey }, { 'apiKey retrieved': k => !!k });

  return { jwt, apiKey };
}

export default function ({ jwt, apiKey }) {
  const res = http.get(`${BASE}/Limited`, {
    headers: {
      Authorization: `Bearer ${jwt}`,
      'X-API-KEY': apiKey,
    },
  });

  check(res, { '200 or 429': r => r.status === 200 || r.status === 429 });
  sleep(1);
}
