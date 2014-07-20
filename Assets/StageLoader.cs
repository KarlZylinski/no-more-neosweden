using System;
using System.Linq;
using UnityEngine;

namespace Assets
{
    public class StageLoader : MonoBehaviour
    {
        public Texture2D Stage;
        public GameObject Player;
        public GameObject MagentaTile;
        public GameObject YellowTile;
        public GameObject OrangeTile;
        public GameObject BlueTile;
        public GameObject[] GroundTiles;
        public GameObject GroundTileEnd;
        public GameObject[] AirTiles;
        public GameObject AirTileEnd;

        public GameObject StripedBackground;
        public GameObject FarBackground;
        public GameObject NearBackground;

        public GameObject[] NearestBackgrounds;

        public void Start()
        {
            CreateWorld();
        }

        private void CreateWorld()
        {
            var player_pixel = new Color(0, 1, 0);
            var ground_pixel = new Color(0, 0, 0);
            var yellow_pixel = new Color(1, 1, 0);
            var blue_pixel = new Color(0, 0, 1);
            var orange_pixel = new Color(1, 0, 0);
            var magenta_pixel = new Color(1, 0, 1);

            var t = DateTime.UtcNow - new DateTime(1970, 1, 1);
            var seconds_since_epoch = (int) t.TotalSeconds;

            var random = new System.Random(seconds_since_epoch);

            for (var x = 0; x < Stage.width; ++x)
            {
                for (var y = Stage.height - 1; y >= 0; --y)
                {
                    var p = Stage.GetPixel(x, y);
                    var r = random.Next();

                    if (p.Equals(player_pixel))
                        Spawn(Player, x, y);
                    /*else if (y == 356 && p.Equals(ground_pixel))
                        SpawnEndPieceOrNormal(x, y, ground_pixel, GroundTiles, GroundTileEnd, r, Stage);*/
                    else if (p.Equals(ground_pixel))
                        SpawnEndPieceOrNormal(x, y, ground_pixel, AirTiles, AirTileEnd, r, Stage);
                    else if (p == yellow_pixel)
                        Spawn(YellowTile, x, y, false, -0.06f);
                    else if (p == blue_pixel)
                        Spawn(BlueTile, x, y, false, -0.06f);
                    else if (p == orange_pixel)
                        SpawnEndPieceOrNormal(x, y, orange_pixel, GroundTiles, GroundTileEnd, r, Stage);
                    else if (p == magenta_pixel)
                        Spawn(MagentaTile, x, y, false, -0.06f);
                }
            }

            var striped_1 = (GameObject)Instantiate(StripedBackground);
            const int scale_x = 100000;
            const float striped_y = 2.8f;
            striped_1.transform.localScale = new Vector3(scale_x, 1);
            striped_1.transform.position = new Vector3(0, striped_y);

            var striped_2 = (GameObject)Instantiate(StripedBackground);
            striped_2.transform.localScale = new Vector3(scale_x, 1);
            striped_2.transform.position = new Vector3(0, striped_y + 180 * 0.16f * 0.16f * 0.32f);

            var striped_3 = (GameObject)Instantiate(StripedBackground);
            striped_3.transform.localScale = new Vector3(scale_x, 1);
            striped_3.transform.position = new Vector3(0, striped_y - 180 * 0.16f * 0.16f * 0.32f);

            var near_bg_width = NearBackground.renderer.bounds.size.x;
            var current_near_bg_x_pos = 0.0f;

            while (current_near_bg_x_pos < Stage.width*0.16f)
            {
                var go = (GameObject)Instantiate(NearBackground, new Vector3(current_near_bg_x_pos, 1.2f, 6), Quaternion.identity);
                go.isStatic = true;
                current_near_bg_x_pos += near_bg_width;
            }

            var far_bg_width = FarBackground.renderer.bounds.size.x;
            var current_far_bg_x_pos = -far_bg_width*0.4f;

            while (current_far_bg_x_pos < Stage.width * 0.16f)
            {
                var go = (GameObject)Instantiate(FarBackground, new Vector3(current_far_bg_x_pos, 1.45f, 10), Quaternion.identity);
                go.isStatic = true;
                current_far_bg_x_pos += far_bg_width;
            }

            var nearest_bg_width = NearestBackgrounds[0].renderer.bounds.size.x;
            var current_nearest_bg_x_pos = 0.0f;

            while (current_nearest_bg_x_pos < Stage.width * 0.16f)
            {
                var go = (GameObject)Instantiate(NearestBackgrounds[random.Next() % NearestBackgrounds.Count()], new Vector3(current_nearest_bg_x_pos, 0.7f, 1), Quaternion.identity);
                go.isStatic = true;
                current_nearest_bg_x_pos += nearest_bg_width;
            }
        }

        public void Update()
        {

        }


        // Implementation.

        static private void SpawnEndPieceOrNormal(int x, int y, Color pixel, GameObject[] tiles, GameObject end, int r, Texture2D stage)
        {
            if (x > 0 && !stage.GetPixel(x - 1, y).Equals(pixel))
                Spawn(end, x, y);
            else if (x < stage.width && !stage.GetPixel(x + 1, y).Equals(pixel))
            {
                Spawn(end, x, y, flip_x: true);
                var x_end = x;
                var x_start = FindGroundStart(x, y, stage, pixel);
                var collider = new GameObject("collider");
                var box = collider.AddComponent<BoxCollider2D>();
                var width = (x_end - x_start);
                box.size = new Vector2(0.16f, 0.15f);
                box.transform.position = new Vector3((x_start + width / 2) * 0.16f, y * 0.16f - 0.1f * 0.16f, 0);
                box.transform.localScale = new Vector3(width, 1);
                box.gameObject.layer = 8;
            }
            else
                Spawn(tiles[r%tiles.Count()], x, y);
        }

        private static int FindGroundStart(int x, int y, Texture2D stage, Color pixel)
        {
            var current_pixel = pixel;
            var current_x = x;

            while (current_pixel.Equals(pixel) && current_x > 0)
            {
                --current_x;
                current_pixel = stage.GetPixel(current_x, y);
            }

            return current_x;
        }

        private static void Spawn(GameObject obj, int x, int y, bool flip_x = false, float offset_y = 0)
        {
            var instance = (GameObject)Instantiate(obj, new Vector3(x * 0.16f, y * 0.16f + offset_y), Quaternion.identity);
            instance.isStatic = true;

            if (flip_x)
                instance.transform.localScale = new Vector3(-1, 1, 1);
        }
    }
}
