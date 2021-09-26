using UnityEngine;

namespace BlueBird.World.Utilities {
    // Inherit to create a single, global-accessible instance of a class, available at all times
    public abstract class _0SingletonManager<T> : MonoBehaviour where T : Component {
        /* Variables */
        private static object _lock = new object(); // Lock for multi-threading protection
        private static bool shuttingDown = false; // Check to see if we are about to be destroyed
        private static bool firstMessage = true;

        // Protected, so no one can accidentally create an Instance
        protected _0SingletonManager() { }
        protected static T _instance;

        // Access Singleton Instance through this Propriety
        public static T Instance {
            get {
                if(shuttingDown) {
                    Debug.LogWarning("<b>Singleton Instance '" + typeof(T).Name + "'</b> <color=yellow><i>already destroyed</i></color>. Returning <color=blue><i>null</i></color>.");
                    return null;
                }

                lock(_lock) {
                    if(_instance == null) {
                        _instance = (T)FindObjectOfType(typeof(T));
                        if(_instance == null) {
                            // Create a GameObject for the Singleton
                            GameObject singletonObject = new GameObject(typeof(T).Name);
                            GameObject parentObject = GameObject.FindWithTag("Director");
                            singletonObject.transform.SetParent(parentObject.transform, false);
                            singletonObject.transform.localPosition = Vector3.zero;
                            singletonObject.transform.localEulerAngles = Vector3.zero;
                            singletonObject.transform.localScale = Vector3.one;
                            singletonObject.tag = "Manager";
                            singletonObject.layer = 7;
                            _instance = singletonObject.AddComponent<T>();
                            Debug.Log("<b>" + typeof(T).Name + " Singleton</b> was <color=green><i>created</i></color>.");
                        }
                    }
                    else if(firstMessage) {
                        Debug.LogWarning("<b>" +  typeof(T).Name + " Singleton</b> has <color=yellow><i>already been created</i></color>.");
                        firstMessage = false;
                    }
                }

                return _instance;
            }
        }

        protected virtual void Awake() {
            if(_instance != null) {
                Destroy(this.gameObject);
                return;
            }
        }

        private void OnApplicationQuit() {
            shuttingDown = true;
            firstMessage = true;
        }

        private void OnDestroy() {
            shuttingDown = true;
            firstMessage = true;
        }
    }
}