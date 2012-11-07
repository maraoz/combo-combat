package ar.com.nuchon.backend.domain.base;


/**
 * Represents the change of a field on a game object, to be applied to a game
 * state to update it
 * 
 * @author maraoz
 * 
 */
public class GameObjectFieldChange {

	private final String fieldName;
	private final Object changedValue;

	public GameObjectFieldChange(String fieldName, Object changedValue) {
		this.fieldName = fieldName;
		this.changedValue = changedValue;
	}
	
	public Object getChangedValue() {
		return changedValue;
	}
	
	public String getFieldName() {
		return fieldName;
	}
}
