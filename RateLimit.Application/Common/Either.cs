namespace RateLimit.Application.Common;

public readonly struct Either<TLeft, TRight>
{
    private readonly TLeft _left;
    private readonly TRight _right;
    private readonly bool _isLeft;

    public Either(TLeft left)
    {
        _left = left;
        _right = default!;
        _isLeft = true;
    }

    public Either(TRight right)
    {
        _left = default!;
        _right = right;
        _isLeft = false;
    }

    public TResult Match<TResult>(
        Func<TLeft, TResult> leftFunc,
        Func<TRight, TResult> rightFunc
    ) => _isLeft ? leftFunc(_left) : rightFunc(_right);

    public static implicit operator Either<TLeft, TRight>(TLeft left) => new Either<TLeft, TRight>(left);
    public static implicit operator Either<TLeft, TRight>(TRight right) => new Either<TLeft, TRight>(right);
}