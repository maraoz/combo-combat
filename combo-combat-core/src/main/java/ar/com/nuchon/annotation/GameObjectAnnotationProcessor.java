package ar.com.nuchon.annotation;

import java.lang.reflect.Field;
import java.util.List;

import ar.com.nuchon.backend.domain.base.GameObject;

import com.google.common.collect.Lists;

public class GameObjectAnnotationProcessor {
	
	public static void process(Class<? extends GameObject> clazz) {
		Field[] fields = clazz.getFields();
		List<Field> networkedFields = Lists.newArrayList();
		for (Field field : fields) {
			if (field.isAnnotationPresent(NetworkData.class)) {
				networkedFields.add(field);
			}
		}
		GameObjectInspector.store(clazz, networkedFields);
	}
}
