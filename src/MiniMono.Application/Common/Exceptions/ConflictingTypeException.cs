namespace Wright.Demo.MiniMono.Application.Common.Exceptions;

/// <summary>
/// Indicates a conflict of type during create/replace operations.
/// </summary>
/// <typeparam name="TType">Type defining Type Discriminators.</typeparam>
/// <typeparam name="TId">Entity Id Type.</typeparam>
public class ConflictingTypeException<TType, TId> : Exception
	where TType : notnull, Enum
	where TId : struct
{
	/// <summary>
	/// Initializes a new instance of the <see cref="ConflictingTypeException{TType, TId}"/> class.
	/// </summary>
	/// <param name="id">Entity ID that caused the conflict.</param>
	/// <param name="expected">Type the entity expected by the client.</param>
	/// <param name="actual">Type of entity on the server.</param>
	public ConflictingTypeException(TId id, TType expected, TType actual)
		: base($"Unable to replace entity '{id}' as it is of type of '{actual.ToEnumMemberValue()}', not '{expected.ToEnumMemberValue()}'.")
	{
		Expected = expected;
		Actual = actual;
	}

	/// <summary>
	/// Gets the actual type of the entity on the server.
	/// </summary>
	public TType Actual { get; }

	/// <summary>
	/// Gets the user expected type of the entity.
	/// </summary>
	public TType Expected { get; }
}