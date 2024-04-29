using System.Collections.Generic;
using System.Linq;

public class ViewManager : SingletonMono<ViewManager>
{
    private List<BaseView> views = new();

    private BaseView currentView;

    protected override void Start()
    {
        base.Start();

        views = GetComponentsInChildren<BaseView>().ToList();

        foreach (var view in views)
        {
            view.Hide();
        }
    }

    public void Show(string viewName)
    {
        var view = views.Find(e => e.viewName == viewName);
        if (view != null)
        {
            if (currentView != null) currentView.Hide();
            view.Show();
            currentView = view;
        }
    }
}
