using UnityEngine;
using BlueBird.World.Director;
using BlueBird.World.Parameters;
using BlueBird.World.Utilities;
using BlueBird.World.Visual;
using BlueBird.World.Player.Character;

namespace BlueBird.World.Player {
    public sealed class PlayerManager : _0SingletonManager<PlayerManager> {
        /* Instances */
        PlayerGenerator _playerGenerator = new PlayerGenerator();
        CharacterGenerator _characterGenerator = new CharacterGenerator();

        protected override void Awake() {
            if(WorldDirector.newGame && !WorldDirector.loadGame) { // Loading New Game
                Debug.Log("<b>PlayerManager Awake</b> function was <color=magenta><i>fired</i></color>.");
                // Generate Player Dictionaries
                _playerGenerator.CreatePlayerDictionaries();
                _playerGenerator.ClearPlayerDictionaries();

                // Creating Player GameObjects
                _playerGenerator.CreatePlayerGameObject(); ;
                DontDestroyOnLoad(WorldData.dictionaryMainObject["Player"]);
                _playerGenerator.CatchMainCameraGameObject();

                _characterGenerator.CreateCharacterGameObject(Settings_str.playerSpawnPosition);
                _characterGenerator.CreateCharacterCameraGameObject(Constants_str.characterCameraHeight);
                _characterGenerator.SetupCharacterGameObject();

            } else if(!WorldDirector.newGame && WorldDirector.loadGame) { // Loading Saved Game
                Debug.Log("<b>PlayerManager Awake</b> function was <color=magenta><i>fired</i></color>.");

            } else { // Error to Awake
                Debug.LogWarning("<b>Booleans newGame or loadGame</b> are <color=orange><i>not valid</i></color>. Couldnt <b><color=yellow><i>'Awake'</i></color> PlayerManager</b>.");
            }
        }

        private void Start() {
            if(WorldDirector.newGame && !WorldDirector.loadGame) { // Loading New Game
                Debug.Log("<b>PlayerManager Start</b> function was <color=magenta><i>fired</i></color>.");

            } else if(!WorldDirector.newGame && WorldDirector.loadGame) { // Loading Saved Game
                Debug.Log("<b>PlayerManager Start</b> function was <color=magenta><i>fired</i></color>.");

            } else { // Error to Start
                Debug.LogWarning("<b>Booleans newGame or loadGame</b> are <color=orange><i>not valid</i></color>. Couldnt <b><color=yellow><i>'Start'</i></color> PlayerManager</b>.");
            }
            WorldDirector.checkerPlayerStart = true;
        }
    }
}
