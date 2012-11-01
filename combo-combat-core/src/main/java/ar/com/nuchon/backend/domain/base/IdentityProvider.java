package ar.com.nuchon.backend.domain.base;

import java.util.concurrent.atomic.AtomicLong;

public class IdentityProvider {

	private static AtomicLong nextId = new AtomicLong(0);
	
	public static Long next() {
		return nextId.incrementAndGet();
	}
	
}
