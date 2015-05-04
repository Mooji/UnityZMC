using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using DG.Tweening;

public class PanelManager : MonoBehaviour
{

    public GameObject scrollView;
    public GameObject contentPanel;

    private bool alignCenter = false;
    private float scrollViewWidth;
    private float totalWidth;
    //value=x:起始坐标, y:宽, z:间隔
    private Dictionary<int, Vector3> panelPositions = new Dictionary<int , Vector3>();
    void Start() {
        RectTransform rt = scrollView.GetComponent<RectTransform>();
        scrollViewWidth = rt.rect.width;
	}

    void Update()
    {

    }

    public void OnScrollChange()
    {
    }

    public void OnClosePanel()
    {

        scrollView.SetActive( !scrollView.activeInHierarchy );

    }

    public void OnRemovePanel( int index )
    {

    }

    public void OnShowPanel( int index )
    {
        if ( !scrollView.activeInHierarchy )
        {
            return;
        }
        if ( index >= panelPositions.Count )
        {
            return;
        }
        showPanel( index );
    }

    public void OnAddPanel( int width )
    {
        if ( !scrollView.activeInHierarchy )
        {
            return;
        }
        addPanel( width , 200f , 10f );
        //显示最后添加的一个
        OnShowPanel( panelPositions.Count - 1 );
    }
    private void showPanel( int index )
    {
        Vector3 v3 = panelPositions[index];
        float curWidth = v3.y + v3.z;

        float x = 0;

        Vector2 contentPos = contentPanel.transform.position;

        if ( !alignCenter )
        {
            float moveWidth = v3.x - getStartX( 0 );
            float distance = moveWidth + contentPos.x;

            if ( moveWidth == 0 )
            {

            }
            else if ( distance > 0 )
            {
                //左移
                RectTransform rt = contentPanel.GetComponent<RectTransform>();
                float tmpWidth = rt.rect.width - ( scrollViewWidth - contentPos.x );
                float tmp = tmpWidth - distance;
                if ( tmp >= 0 )
                {
                    x = contentPos.x - distance;
                }
                else
                {
                    x = contentPos.x - tmpWidth;
                }
            }
            else
            {
                //右移
                float tmp = contentPos.x - distance;
                if ( tmp < 0 )
                {
                    x = tmp;
                }
            }

        }


        if ( alignCenter )
        {

            float viewCenterx = scrollViewWidth / 2f - contentPos.x - curWidth / 2f + getStartX( 0 );

            float distance = viewCenterx - v3.x;
            if ( distance == 0f )
                return;

            if ( distance > 0 )
            {
                //右移
                float tmp = distance + contentPos.x;
                if ( tmp < 0 )
                {
                    x = tmp;
                }
            }
            else
            {
                //左移
                RectTransform rt = contentPanel.GetComponent<RectTransform>();
                float tmpWidth = rt.rect.width - ( scrollViewWidth - contentPos.x );
                float tmp = distance + tmpWidth;
                if ( tmp >= 0 )
                {
                    x = contentPos.x + distance;
                }
                else
                {
                    x = contentPos.x - tmpWidth;
                }
            }
        }
        contentPanel.transform.DOMoveX( x , 1f );
        //contentPanel.transform.position = new Vector2(x, contentPos.y);
    }
    private void addPanel( float width , float height , float space )
    {
        int curIndex = panelPositions.Count;
        GameObject panel = getPanel();
        Text text = panel.GetComponentInChildren<Text>();
        text.text = "panel " + curIndex;
        panel.transform.SetParent( this.contentPanel.gameObject.transform );

        RectTransform parentRect = this.contentPanel.gameObject.GetComponent<RectTransform>();
        float parentWidth = parentRect.rect.width;
        float parentHeight = parentRect.rect.height;

        RectTransform rt = panel.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2( width , height );

        float tmpWidth = width + space;
        totalWidth += tmpWidth;
        if ( totalWidth > parentWidth )
        {
            expandParent( totalWidth , parentHeight );
        }
        float startX = getStartX( width );
        float x = totalWidth - tmpWidth + startX;
        rt.localPosition = new Vector3( x , 0 , 0 );

        panelPositions.Add( curIndex , new Vector3( x , width , space ) );
    }

    private float getStartX( float width )
    {
        float startX = panelPositions.Count == 0 ? width / 2f : panelPositions[0].x;
        return startX;
    }

    private void expandParent( float width , float height )
    {
        RectTransform rt = this.contentPanel.gameObject.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2( width , height );
    }

    private GameObject getPanel()
    {
        Object obj = Resources.Load( "Prefabs/Demo/empty_panel" );
        GameObject go = Instantiate( obj ) as GameObject;
        return go;
    }
}
