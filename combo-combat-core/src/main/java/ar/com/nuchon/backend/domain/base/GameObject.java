package ar.com.nuchon.backend.domain.base;

import ar.com.nuchon.annotation.NetworkData;

import com.google.common.base.Preconditions;

public class GameObject {

	private final Long id;
	
	@NetworkData
	private boolean destroyMe = false;

	public GameObject() {
		this.id = IdentityProvider.next();
	}
	
	public GameObject(Long id) {
		Preconditions.checkNotNull(id);
		this.id = id;
	}
	
	public GameObject(GameObject other) {
		Preconditions.checkNotNull(other);
		id = other.id;
		destroyMe = other.destroyMe;
	}

	public Long getId() {
		return id;
	}
	
	public boolean shouldDestroyMe() {
		return destroyMe;
	}
	
	public void setDestroyMe(boolean value) {
		this.destroyMe = value;
	}

	@Override
	public int hashCode() {
		final int prime = 31;
		int result = 1;
		result = prime * result + ((id == null) ? 0 : id.hashCode());
		return result;
	}

	@Override
	public boolean equals(Object obj) {
		if (this == obj)
			return true;
		if (obj == null)
			return false;
		if (getClass() != obj.getClass())
			return false;
		GameObject other = (GameObject) obj;
		if (id == null) {
			if (other.id != null)
				return false;
		} else if (!id.equals(other.id))
			return false;
		return true;
	}
	
	
	

}
