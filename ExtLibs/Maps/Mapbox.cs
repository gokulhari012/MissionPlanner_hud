﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MissionPlanner.Maps
{
    using System;
    using GMap.NET.Projections;
    using System.Globalization;
    using GMap.NET.MapProviders;
    using GMap.NET;
    using System.Reflection;
    using MissionPlanner.Utilities;

    /// <summary>
    /// EarthBuilder Custom
    /// </summary>
    public class MapBox : GMapProvider
    {
        public static readonly MapBox Instance;

        MapBox()
        {
            MaxZoom = 24;
        }

        static MapBox()
        {
            Instance = new MapBox();

            Type mytype = typeof (GMapProviders);
            FieldInfo field = mytype.GetField("DbHash", BindingFlags.Static | BindingFlags.NonPublic);
            Dictionary<int, GMapProvider> list = (Dictionary<int, GMapProvider>) field.GetValue(Instance);

            list.Add(Instance.DbId, Instance);
        }

        #region GMapProvider Members

        readonly Guid id = new Guid("22d6e496-842b-4549-bd6d-8f019e6c019f");

        public override Guid Id
        {
            get { return id; }
        }

        readonly string name = "MapBox Satellite";

        public override string Name
        {
            get { return name; }
        }

        GMapProvider[] overlays;

        public override GMapProvider[] Overlays
        {
            get
            {
                if (overlays == null)
                {
                    overlays = new GMapProvider[] {this};
                }
                return overlays;
            }
        }

        public override PureProjection Projection
        {
            get { return MercatorProjection.Instance; }
        }

        public override PureImage GetTileImage(GPoint pos, int zoom)
        {
            string url = MakeTileImageUrl(pos, zoom, LanguageStr);

            return GetTileImageUsingHttp(url);
        }

        #endregion

        string MakeTileImageUrl(GPoint pos, int zoom, string language)
        {
            string ret;

            ret = string.Format(CultureInfo.InvariantCulture, CustomURL, pos.X, pos.Y, zoom, mapsource, mapkey);

            return ret;
        }

        public static string CustomURL = "http://a.tiles.mapbox.com/v4/{3}/{2}/{0}/{1}.png?access_token={4}";

        public object mapsource = "mapbox.satellite";
        public object mapkey = Settings.Instance.GetString("MapBoxAPIKey");
    }
}