package ar.com.nuchon.annotation;

import java.lang.reflect.Field;
import java.util.Set;

import org.reflections.Reflections;

import ar.com.nuchon.backend.domain.base.GameObject;

public class GameObjectAnnotationProcessor {
	
	
	public static void processDomainClasses() {
		Reflections reflections = new Reflections("ar.com.nuchon.backend.domain");
		Set<Class<? extends GameObject>> gameObjectClasses = reflections.getSubTypesOf(GameObject.class);
		for (Class<? extends GameObject> clazz : gameObjectClasses) {
			System.out.println("Inspecting networking annotations on GameObject class "+clazz.getSimpleName());
			process(clazz);
		}
	}
	
	public static void process(Class<? extends GameObject> clazz) {
		Set<Field> networkedFields = Reflections.getAllFields(clazz, Reflections.withAnnotation(NetworkData.class));
		for (Field field : networkedFields) {
			field.setAccessible(true);
			System.out.println("\tFound network data: "+field.getName());
		}
		GameObjectInspector.store(clazz, networkedFields);
	}

	public static void setField(GameObject gameObject, String fieldName,
			Object changedValue) {
		try {
			Field field = gameObject.getClass().getField(fieldName);
			field.setAccessible(true);
			field.set(gameObject, changedValue);
		} catch (SecurityException e) {
			e.printStackTrace();
		} catch (NoSuchFieldException e) {
			e.printStackTrace();
		} catch (IllegalArgumentException e) {
			e.printStackTrace();
		} catch (IllegalAccessException e) {
			e.printStackTrace();
		}
		
	}
	
	
	
}