using System;
using ARAM.Main.Characters;
using UniRx;
using UnityEngine;
using Zenject;

namespace ARAM.Main.Managers
{
    public class CharacterSpawner : MonoBehaviour
    {
        [Inject] private GameStateManager _gameStateManager;
        [Inject] private ARProvider _arProvider;
        [SerializeField] private Character characterPrefab;
        [SerializeField] private ParticleSystem spawnParticle;
        private Subject<Character> _spawnCharacterSubject = new Subject<Character>();
        public IObservable<Character> SpawnCharacterObservable => _spawnCharacterSubject.AsObservable();
        

        private void Start()
        {
            _arProvider.SpawnCharacterObservable
                .Subscribe(SpawnCharacter)
                .AddTo(this);
        }

        private void SpawnCharacter(Vector3 position)
        {
            Instantiate(spawnParticle);
            var character = Instantiate(characterPrefab);
            character.transform.position = position;
            _spawnCharacterSubject.OnNext(character);
        }
    }
}