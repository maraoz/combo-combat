package ar.com.nuchon.annotation;

import java.lang.reflect.Field;
import java.util.Map;
import java.util.Set;

import ar.com.nuchon.backend.domain.base.GameObject;

import com.google.common.collect.Maps;


public class GameObjectInspector {
	
	private static final Map<Class<? extends GameObject>, Set<Field>> fieldsForClass = Maps.newHashMap();

	public static void store(Class<? extends GameObject> clazz,
			Set<Field> networkedFields) {
		fieldsForClass.put(clazz, networkedFields);
	}
	
	public static Set<Field> getNetworkedFields(Class<? extends GameObject> clazz) {
		return fieldsForClass.get(clazz);
	}

	
	
	
}
