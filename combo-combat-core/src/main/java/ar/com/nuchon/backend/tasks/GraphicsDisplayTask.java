package ar.com.nuchon.backend.tasks;

import com.badlogic.gdx.ApplicationListener;
import com.badlogic.gdx.Gdx;
import com.badlogic.gdx.graphics.GL10;
import com.badlogic.gdx.graphics.OrthographicCamera;
import com.badlogic.gdx.graphics.Texture;
import com.badlogic.gdx.graphics.Texture.TextureFilter;
import com.badlogic.gdx.graphics.g2d.Sprite;
import com.badlogic.gdx.graphics.g2d.SpriteBatch;
import com.badlogic.gdx.graphics.g2d.TextureRegion;

public class GraphicsDisplayTask extends SynchedTask implements ApplicationListener{

	private static final int MOUSE_NETWORK_RESOLUTION = 4;
	private static final int PLAYER_SIZE = 10;
	private static final int BULLET_SIZE = 3;

	/*public void run() {
		try {
			Display.setDisplayMode(new DisplayMode(800, 600));
			Display.create();
		} catch (LWJGLException e) {
			e.printStackTrace();
		}

		initGL(); // init OpenGL
		getDelta(); // call once before loop to initialise lastFrame
		lastFPS = getTime(); // call before loop to initialise fps timer

		int frame = 0;
		while (!Display.isCloseRequested()) {


			update(getDelta());
			ClientBackend.update();

			int x = 0;
			int y = 0;

			if( Keyboard.isKeyDown(Keyboard.KEY_D) )
				x += 1;
			if( Keyboard.isKeyDown(Keyboard.KEY_A) )
				x -= 1;
			if( Keyboard.isKeyDown(Keyboard.KEY_W) )
				y += 1;
			if( Keyboard.isKeyDown(Keyboard.KEY_S) )
				y -= 1;
			if( y!= 0 || x!=0 )
				MessageHub.route(new MoveKeyNotify(new Vector2D(x,y)));
			
			boolean leftButtonDown = Mouse.isButtonDown(0);
			if (leftButtonDown) {
				MessageHub.route(new ShootKeyNotify(getMousePos()));
			}
			
			
			if (frame % MOUSE_NETWORK_RESOLUTION == 0) {
				//MessageHub.route(new MouseMovedNotify(getMousePos());
			}
			frame++;
			
			
			
			renderGL();
			Display.update();
			waitFPS();
		}

		MessageHub.route(new CloseNotify());
		Display.destroy();

	}

	private Vector2D getMousePos() {
		return new Vector2D(Mouse.getX(), Mouse.getY());
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
		long time = getTime();
		int delta = (int) (time - lastFrame);
		lastFrame = time;

		return delta;
	}

	public long getTime() {
		return (Sys.getTime() * 1000) / Sys.getTimerResolution();
	}

	public void updateFPS() {
		if (getTime() - lastFPS > 1000) {
			Display.setTitle("FPS: " + fps);
			fps = 0;
			lastFPS += 1000;
		}
		fps++;
	}

	public void initGL() {
		GL11.glMatrixMode(GL11.GL_PROJECTION);
		GL11.glLoadIdentity();
		GL11.glOrtho(0, 800, 600, 0, 1, -1);
		GL11.glMatrixMode(GL11.GL_MODELVIEW);
	}

	public void renderGL() {
		// Clear The Screen And The Depth Buffer
		GL11.glClear(GL11.GL_COLOR_BUFFER_BIT | GL11.GL_DEPTH_BUFFER_BIT);

		// R,G,B,A Set The Color To Blue One Time Only
		GL11.glColor3f(0.5f, 0.5f, 1.0f);

		// draw players
		for (Vector2D p : ClientBackend.getPositions()) {
			drawRect(p.getX(), p.getY(), PLAYER_SIZE);
		}
		
		// draw bullets
		for (Bullet b : ClientBackend.getBullets()) {
			drawRect(b.getPos().getX(), b.getPos().getY(), BULLET_SIZE);
		}
	}

	private void drawRect(float x, float wy, float size) {
		
		float y = Display.getHeight() - wy;
		
		GL11.glPushMatrix();
		GL11.glTranslatef(x, y, 0);
		GL11.glRotatef(rotation, 0f, 0f, 1f);
		GL11.glTranslatef(-x, -y, 0);
		
		GL11.glBegin(GL11.GL_QUADS);
		GL11.glVertex2f(x - size , y - size);
		GL11.glVertex2f(x + size, y - size);
		GL11.glVertex2f(x + size, y + size);
		GL11.glVertex2f(x - size, y + size);
		GL11.glEnd();
		GL11.glPopMatrix();
	}
	*/
	
	private OrthographicCamera camera;
	private SpriteBatch batch;
	private Texture texture;
	private Sprite sprite;
	
	public void create() {		
		float w = Gdx.graphics.getWidth();
		float h = Gdx.graphics.getHeight();
		
		camera = new OrthographicCamera(1, h/w);
		batch = new SpriteBatch();
		
		texture = new Texture(Gdx.files.internal("data/libgdx.png"));
		texture.setFilter(TextureFilter.Linear, TextureFilter.Linear);
		
		TextureRegion region = new TextureRegion(texture, 0, 0, 512, 275);
		
		sprite = new Sprite(region);
		sprite.setSize(0.9f, 0.9f * sprite.getHeight() / sprite.getWidth());
		sprite.setOrigin(sprite.getWidth()/2, sprite.getHeight()/2);
		sprite.setPosition(-sprite.getWidth()/2, -sprite.getHeight()/2);
	}

	public void dispose() {
		batch.dispose();
		texture.dispose();
	}

	public void render() {		
		Gdx.gl.glClearColor(1, 1, 1, 1);
		Gdx.gl.glClear(GL10.GL_COLOR_BUFFER_BIT);
		
		batch.setProjectionMatrix(camera.combined);
		batch.begin();
		sprite.draw(batch);
		batch.end();
	}

	public void resize(int width, int height) {
	}

	public void pause() {
	}

	public void resume() {
	}

}
