package ar.com.nuchon.backend.domain.base;

public class GameObject {

	private final Long id;

	public GameObject() {
		this.id = IdentityProvider.next();
	}

	public Long getId() {
		return id;
	}

}
