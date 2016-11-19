using UnityEngine;

namespace BDMk22Plugin
{
    public class UvTransformer
    {
        private readonly Vector2[] _modifiedUv;

        private readonly Vector2[] _origUv;

        public GameObject GameObject;
        public Mesh Mesh;
        public Vector2 TextureSize;


        public UvTransformer(GameObject meshObject)
        {
            GameObject = meshObject;
            Mesh = meshObject.GetComponent<MeshFilter>().mesh;

            _origUv = new Vector2[Mesh.uv.Length];
            _modifiedUv = new Vector2[Mesh.uv.Length];
            for (var i = 0; i < _origUv.Length; i++)
                _origUv[i] = Mesh.uv[i];

            Debug.Log("uv vert count: " + _origUv.Length);

            var texture = (Texture2D) meshObject.GetComponent<MeshRenderer>().material.mainTexture;
            TextureSize = new Vector2(texture.width, texture.height);
        }

        public void UpdateUvTransformation(Vector2 initialOffset, float rotation, Vector2 rotationPivot, Vector2 shift)
        {
            for (var i = 0; i < _origUv.Length; i++)
            {
                var textureSpace = Vector2.Scale(_origUv[i], TextureSize);
                textureSpace += initialOffset;
                textureSpace = Utils.RotateAroundPoint(textureSpace, rotationPivot, rotation);
                textureSpace += shift;

                var uvSpace = new Vector2(textureSpace.x/TextureSize.x, textureSpace.y/TextureSize.y);
                _modifiedUv[i] = uvSpace;
            }
            Mesh.uv = _modifiedUv;
        }
    }
}