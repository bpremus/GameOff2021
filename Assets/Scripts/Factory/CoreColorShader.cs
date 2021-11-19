using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoreColorShader : MonoBehaviour
{
    /*
    private MaterialPropertyBlock _block;
    private Renderer[] _renderer;

    protected virtual void Awake()
    {
        // this will alow us to change the color on material that is shared with all humans
        _block = new MaterialPropertyBlock();
        _renderer = GetComponentsInChildren<Renderer>();
    }
    protected virtual void Update()
    {
        SetHighhlight();
    }

    float _hightlightPower = 0;
    float _highlight_multi = 50f;
    protected void SetHighhlight()
    {
        if (_hightlightPower > 0)
        {
            _block.SetFloat("_bugGlow", _hightlightPower);
            for (int i = 0; i < _renderer.Length; i++)
            {
                _renderer[i].SetPropertyBlock(_block);
            }
            
            _hightlightPower -= Time.deltaTime * _highlight_multi;
        }       
    }
    public void OnHighlight()
    {
        _hightlightPower += Time.deltaTime * _highlight_multi * 2;
        if (_hightlightPower > 5)
            _hightlightPower = 5;
    }
    */
 
}

