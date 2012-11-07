package ar.com.nuchon.annotation;

import java.lang.reflect.Field;
import java.util.List;
import java.util.Map;

import ar.com.nuchon.backend.domain.base.GameObject;

import com.google.common.collect.Maps;


public class GameObjectInspector {
	
	private static final Map<Class<? extends GameObject>, List<Field>> fieldsForClass = Maps.newHashMap();

	public static void store(Class<? extends GameObject> clazz,
			List<Field> networkedFields) {
		fieldsForClass.put(clazz, networkedFields);
	}

	
	
	
}
