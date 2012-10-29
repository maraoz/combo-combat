package ar.com.nuchon.backend.tasks;

import ar.com.nuchon.backend.ClientBackend;
import ar.com.nuchon.backend.domain.Bullet;
import ar.com.nuchon.backend.domain.Vector2D;
import ar.com.nuchon.network.dispatch.MessageHub;
import ar.com.nuchon.network.message.gui.CloseNotify;
import ar.com.nuchon.network.message.gui.MoveKeyNotify;
import ar.com.nuchon.network.message.gui.ShootKeyNotify;

import com.badlogic.gdx.ApplicationListener;
import com.badlogic.gdx.Gdx;
import com.badlogic.gdx.Input.Buttons;
import com.badlogic.gdx.Input.Keys;
import com.badlogic.gdx.graphics.GL10;
import com.badlogic.gdx.graphics.OrthographicCamera;
import com.badlogic.gdx.graphics.Texture;
import com.badlogic.gdx.graphics.Texture.TextureFilter;
import com.badlogic.gdx.graphics.g2d.Sprite;
import com.badlogic.gdx.graphics.g2d.SpriteBatch;
import com.badlogic.gdx.graphics.g2d.TextureRegion;
import com.badlogic.gdx.graphics.glutils.ShapeRenderer;
import com.badlogic.gdx.graphics.glutils.ShapeRenderer.ShapeType;

public class GraphicsDisplayTask extends SynchedTask implements
		ApplicationListener {

	private static final int PLAYER_SIZE = 10;
	private static final int BULLET_SIZE = 3;

	private ShapeRenderer shapeRenderer;

	private void drawRect(float x, float wy, float size) {
		shapeRenderer.begin(ShapeType.Rectangle);
		shapeRenderer.setColor(0, 0, 0, 1);
		shapeRenderer.identity();
		shapeRenderer.translate(x, wy, 0);
		shapeRenderer.rotate(0, 0, 1, 90);
		shapeRenderer.rect(-size / 2, -size / 2, size, size);
		shapeRenderer.end();
	}

	private Vector2D getMousePos() {

		return new Vector2D(Gdx.input.getX(), Gdx.graphics.getHeight()-Gdx.input.getY());

	}

	float rotation = 0;

	long lastFrame;

	int fps;
	long lastFPS;

	public void update(int delta) {
		// rotate quad
		rotation += 0.15f * delta;

		updateFPS(); // update FPS Counter
	}

	public int getDelta() {
		long time = System.currentTimeMillis();
		int delta = (int) (time - lastFrame);
		lastFrame = time;

		return delta;
	}

	public void updateFPS() {
		if (System.currentTimeMillis() - lastFPS > 1000) {
			fps = 0;
			lastFPS += 1000;
		}
		fps++;
	}

	private OrthographicCamera camera;
	private SpriteBatch batch;
	private Texture texture;
	private Sprite sprite;

	public void create() {
		float w = Gdx.graphics.getWidth();
		float h = Gdx.graphics.getHeight();

		shapeRenderer = new ShapeRenderer();

		camera = new OrthographicCamera(1, h / w);
		batch = new SpriteBatch();

		texture = new Texture(Gdx.files.internal("data/libgdx.png"));
		texture.setFilter(TextureFilter.Linear, TextureFilter.Linear);

		TextureRegion region = new TextureRegion(texture, 0, 0, 512, 275);

		sprite = new Sprite(region);
		sprite.setSize(0.9f, 0.9f * sprite.getHeight() / sprite.getWidth());
		sprite.setOrigin(sprite.getWidth() / 2, sprite.getHeight() / 2);
		sprite.setPosition(-sprite.getWidth() / 2, -sprite.getHeight() / 2);
	}

	public void dispose() {
		MessageHub.route(new CloseNotify());
		batch.dispose();
		texture.dispose();
	}

	public void render() {

		// process
		update(getDelta());
		ClientBackend.update();

		int x = 0;
		int y = 0;

		if (Gdx.input.isKeyPressed(Keys.D))
			x += 1;
		if (Gdx.input.isKeyPressed(Keys.A))
			x -= 1;
		if (Gdx.input.isKeyPressed(Keys.W))
			y += 1;
		if (Gdx.input.isKeyPressed(Keys.S))
			y -= 1;
		if (y != 0 || x != 0)
			MessageHub.route(new MoveKeyNotify(new Vector2D(x, y)));

		boolean leftButtonDown = Gdx.input.isButtonPressed(Buttons.LEFT);
		if (leftButtonDown) {
			MessageHub.route(new ShootKeyNotify(getMousePos()));
		}

		waitFPS();

		// draw
		// Clear The Screen And The Depth Buffer
		Gdx.gl.glClearColor(1, 1, 1, 1);
		Gdx.gl.glClear(GL10.GL_COLOR_BUFFER_BIT);

		// draw players
		for (Vector2D p : ClientBackend.getPositions()) {
			drawRect(p.getX(), p.getY(), PLAYER_SIZE);
		}

		// draw bullets
		for (Bullet b : ClientBackend.getBullets()) {
			drawRect(b.getPos().getX(), b.getPos().getY(), BULLET_SIZE);
		}

		batch.setProjectionMatrix(camera.combined);
		batch.begin();
		//sprite.draw(batch);
		batch.end();
	}

	public void resize(int width, int height) {
	}

	public void pause() {
	}

	public void resume() {
	}

}
