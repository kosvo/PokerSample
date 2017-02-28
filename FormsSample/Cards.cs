using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using Urho;
using Urho.Actions;
using Urho.Gui;
using Urho.Shapes;

namespace FormsSample
{
	public class Cards : Application
	{
		bool movementsEnabled;
		Scene scene;
		Node plotNode;
		Camera camera;
		Octree octree;


		[Preserve]
		public Cards(ApplicationOptions options = null) : base(options)
		{

		}

		static Cards()
		{
			UnhandledException += (s, e) =>
			{
				if (Debugger.IsAttached)
					Debugger.Break();
				e.Handled = true;
			};
		}

		protected override void Start()
		{
			base.Start();
			CreateScene();
			SetupViewport();
		}

		async void CreateScene()
		{
			var cache = ResourceCache;
			scene = new Scene();
			octree = scene.CreateComponent<Octree>();

			plotNode = scene.CreateChild();

			var baseNode = plotNode.CreateChild().CreateChild();
			var plane = baseNode.CreateComponent<StaticModel>();
			plane.Model = ResourceCache.GetModel("Models/Plane.mdl");

			var cameraNode = scene.CreateChild("camera");
			camera = cameraNode.CreateComponent<Camera>();
			cameraNode.Position = new Vector3(10, 15, 10) / 1.75f;
			cameraNode.Rotation = new Quaternion(-0.121f, 0.878f, -0.305f, -0.35f);

			await plotNode.RunActionsAsync(new EaseBackOut(new RotateBy(2f, 360, 360, 0)));
			movementsEnabled = true;
		}

		protected override void OnUpdate(float timeStep)
		{
			if (Input.NumTouches >= 1 && movementsEnabled)
			{
				var touch = Input.GetTouch(0);
				plotNode.Rotate(new Quaternion(0, -touch.Delta.Y, 0), TransformSpace.Local);
			}
			base.OnUpdate(timeStep);
		}

		public void Rotate(float toValue)
		{
			plotNode.Rotate(new Quaternion(0, toValue, toValue), TransformSpace.Local);
		}

		void SetupViewport()
		{
			var renderer = Renderer;
			renderer.SetViewport(0, new Viewport(Context, scene, camera, null));
		}
	}


}