namespace Mapbox.Unity.Location
{
	using System;
	using System.Collections.Generic;
	using System.Globalization;
	using System.IO;
	using Mapbox.Unity.Utilities;
	using Mapbox.Utils;
	using UnityEngine;
	using Mapbox.Unity.Map;

	/// <summary>
	/// <para>
	/// The EditorLocationProvider is responsible for providing mock location data via log file collected with the 'LocationProvider' scene
	/// </para>
	/// </summary>
	public class EditorLocationProviderLocationLog : AbstractEditorLocationProvider
	{


		/// <summary>
		/// The mock "latitude, longitude" location, respresented with a string.
		/// You can search for a place using the embedded "Search" button in the inspector.
		/// This value can be changed at runtime in the inspector.
		/// </summary>
		[SerializeField]
		private TextAsset _locationLogFile;


		private LocationLogReader _logReader;
		private IEnumerator<Location> _locationEnumerator;


#if UNITY_EDITOR
		protected override void Awake()
		{
			base.Awake();
			_logReader = new LocationLogReader(_locationLogFile.bytes);
			_locationEnumerator = _logReader.GetLocations();
		}
#endif


		private void OnDestroy()
		{
			if (null != _locationEnumerator)
			{
				_locationEnumerator.Dispose();
				_locationEnumerator = null;
			}
			if (null != _logReader)
			{
				_logReader.Dispose();
				_logReader = null;
			}
		}


        public static Vector2d current_latlong = new Vector2d(42.291176846950194, -83.71745116562175);

        public static float movement_magnitude = 0.00001f;
		public AbstractMap map;

		protected override void SetLocation()
		{
            Location fake_location = new Location();
            fake_location.LatitudeLongitude = current_latlong;
            _currentLocation = fake_location;
            //Debug.Log(_currentLocation.LatitudeLongitude);
            //Debug.Log(current_latlong);
        }

		public static void MoveRight()
        {
			current_latlong += new Vector2d(0, movement_magnitude);
			//Debug.Log(current_latlong);
		}

		public static void MoveLeft()
        {
			current_latlong += new Vector2d(0, -movement_magnitude);
			//Debug.Log(current_latlong);
		}

		public static void MoveUp()
        {
			current_latlong += new Vector2d(movement_magnitude, 0);
			//Debug.Log(current_latlong);
		}

		public static void MoveDown()
        {
			current_latlong += new Vector2d(-movement_magnitude, 0);
			//Debug.Log(current_latlong);
		}

		private void Update()
        {
			if (Input.GetKey(KeyCode.RightArrow))
				MoveRight();
			if (Input.GetKey(KeyCode.LeftArrow))
				MoveLeft();
			if (Input.GetKey(KeyCode.UpArrow))
				MoveUp();
			if (Input.GetKey(KeyCode.DownArrow))
				MoveDown();

			transform.position = map.GeoToWorldPosition(current_latlong);
		}

		private void Start()
        {
			Debug.Log(transform.position);
        }


	}
}
