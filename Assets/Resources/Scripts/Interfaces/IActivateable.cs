
/// <summary>
/// Activatables are done throught player actions that are not the interact button
/// i.e. something that is squirted with water, a button that needs to be stood on, or a door that 
/// opens when a water tank(s) is full
/// </summary>
public interface IActivateable 
{
    public void OnActivation();
}
