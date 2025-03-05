using System;
using System.Collections.Generic;
using UnityEngine;

namespace LuniLib.View
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class Animator2D : MonoBehaviour
    {
        #region Fields

        [SerializeField] private AnimationData _defaultAnimation;
        [SerializeField] private AnimationData[] _animationData;
        
        private SpriteRenderer _spriteRenderer;
        private AnimationData _currentAnimation;
        private float _currentTimer;
        private int _currentSpriteIndex;
        private bool _actionIsPlaying;
        private Action _onActionCompleted;
        private string _nextStateName;
        
        private readonly Dictionary<string, AnimationData> _stateDictionary = new();
        private readonly Dictionary<string, AnimationData> _actionDictionary = new();

        #endregion

        public void Start()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();

            foreach (AnimationData animation in _animationData)
            {
                if(animation.AnimationType == AnimationType.STATE)
                    _stateDictionary.TryAdd(animation.AnimationName, animation);
                else
                    _actionDictionary.TryAdd(animation.AnimationName, animation);                    
            }
            
            PlayStateAnimation(_defaultAnimation);
        }

        public void Update()
        {
            Animate();
        }

        private void Animate()
        {
            if (_currentAnimation == null)
                return;

            _currentTimer -= Time.deltaTime;
                
            if (_currentTimer <= 0)
            {   
                _currentTimer = 1f / _currentAnimation.FramePerSeconds;
                _currentSpriteIndex++;

                EndAnimation();
                
                if (_currentAnimation != null) 
                    _spriteRenderer.sprite = _currentAnimation.Sprites[_currentSpriteIndex];
            }
        }

        private void EndAnimation()
        {
            if (_currentSpriteIndex >= _currentAnimation.Sprites.Length)
            {
                _currentSpriteIndex = 0;
                StopAction();
            }
        }

        public AnimationData GetStateAnimationByName(string name)
        {
            return _stateDictionary[name];
        }
        
        public AnimationData GetActionAnimationByName(string name)
        {
            return _actionDictionary[name];
        }

        private void StopAction()
        {
            if (_currentAnimation.AnimationType == AnimationType.ACTION)
            {
                _actionIsPlaying = false;

                if (_onActionCompleted != null)
                {
                    _onActionCompleted?.Invoke();
                    _onActionCompleted = null;
                }

                if (!string.IsNullOrEmpty(_nextStateName))
                {
                    TryPlayAnimation(GetStateAnimationByName(_nextStateName));
                    return;
                }
                    
                if (_defaultAnimation != null)
                    TryPlayAnimation(_defaultAnimation);
                else
                    _currentAnimation = null;
            }
        }


        #region Players

        public void PlayStateAnimation(string animationName)
        {
            TryPlayAnimation(animationName, AnimationType.STATE);
        }
        
        public void PlayStateAnimation(AnimationData animationData)
        {
            TryPlayAnimation(animationData);
        }

        public void PlayActionAnimation(string animationName, string nextState = null, Action onComplete = null)
        {
            if (!TryPlayAnimation(animationName, AnimationType.ACTION)) 
                return;
            
            _actionIsPlaying = true;
            _nextStateName = nextState;
            
            if(onComplete != null)
                _onActionCompleted = onComplete;
        }
        
        private bool TryPlayAnimation(string animationName, AnimationType animationType)
        {
            AnimationData animationData = null;

            
            if (animationType == AnimationType.STATE)
            {
                if (_actionIsPlaying)
                    return false;
                
                if (_currentAnimation == _stateDictionary[animationName])
                    return false;
                
                animationData = _stateDictionary[animationName];
            }
            else if (animationType == AnimationType.ACTION)
            {
                animationData = _actionDictionary[animationName];
            }
            
            return TryPlayAnimation(animationData);
        }

        private bool TryPlayAnimation(AnimationData animationData)
        {
            if (animationData == null)
                return false;
            
            SwitchAnimation(animationData);
            
            return true;
        }

        #endregion

        private void SwitchAnimation(AnimationData newAnimation)
        {
            _currentAnimation = newAnimation;
            _currentSpriteIndex = 0;
            _currentTimer = 1f / newAnimation.FramePerSeconds;
        }
    }
}