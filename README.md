# Introduction
* [Git Setup](git-setup.wiki)
* [Suggested Approach](suggested-approach.md)

# Minimal Features (**more or less final, there will be no big additional requirements **)
* World view
    * Each species should be distinguishable is some way. The most straightforward approach is to use separate colors.
    * Responsiveness: use ViewBoxes and/or ScrollViewers
* [Parameter View](parameter-view.md): the boid's behavior must be modifiable. You must deal with at least all the RangedDoubleParameters. There are also string parameters, but you are allowed to filter them out if you want.
    * Do not hardcode parameters. The Bindings class gives you a list of all parameters, so write code that relies on this list to build the view. An ItemsControl can help.
* It should be possible to add boids in some ways.
    * The position of the new boid can be handled in multiple ways. Implementing one way suffices. For example,
        * When pushing on a Button 'Add Boid', a new boid gets placed somewhere randomly
        * When clicking on the world view, a boid gets added at that position
    * The species of the new boid can be specified in different ways. Again, implementing one approach suffices. For example,
        * Hard code left/right click for prey/hunter. This is of course only possible if there are only two species.
        * You offer the user a list of species in a ListBox, and the new boid's species is determined by whatever species is currently selected.
* At least one model extension
    * Add a new species
    * Add a new type of force
    * Make the species configurable in a data file (e.g. XML)
    * Boids being eaten
    * Boids procreating
    * ...
* The simulation should be pausable

# Feature Suggestions
* Modifiable world size
* [Skinning](skinning.md)
* [Zoomable world view](zoomable-world-view.md)
* Arbitrary simulation speed
* Full screen mode
* User-customizable species colors
* Velocity visualization
* Visual boid selection (showing which boids' parameters are shown)
* Adding new species
    * Hard coded
    * At runtime
* Saving/loading parameters to/from file
* ...


No MVVM violations are permitted. Here's a few things you need to look out for:

* The M should remain fully ignorant of the VM and V.
* The VM should remain fully ignorant of the V, but it does know the M.
* The VM should not expose M-*types* to the V, but M-*objects* are allowed. For example, the VM should not have a property of type `Boid`, because `Boid` is an M-type. However, if the M gives access to a `Cell<double>`, the VM can take this object and give it to the V. The reasoning behind this is that the V is does not **depend** on the fact that this `Cell<double>` originates from the M. If the M were to change and not expose this `Cell<double>` anymore, the VM can hide this change by creating a `Cell<double>` on its own, and the V will be none the wiser.
* The VM is in no way allowed to contain text that is shown in the V. For example, the V could have support for multiple languages, and the same VM should be usable.
  For example, say the VM needs to tell the V whether the simulation is paused or not, do not use strings "Paused" and "Running", but use a bool IsPaused.
  The bool then gets translated to "Paused" and "Running" by the V, probably using an IValueConverter.
* DispatcherTimer is a WPF specific class. Do NOT use it in the VM. Possible solutions:
    * The VM offers an Update(double ms) method that advances the simulation by ms. This method is then called by the V every N milliseconds.
    * Let the VM handle the timer itself, but hide the DispatcherTimer behind an ITimerService interface.
* No colors allowed anywhere else than in the V, not even a 'red' string in the VM/M. The M and VM only know species and must not know anything about visual concepts such as colors.
  If you wish to associate colors with species, this must be handled in the V.
* The code behind file (e.g. MainWindow.xaml.cs) is allowed to contain V-specific code. For example, if you need to deal with `MouseDown` or `MouseWheel` events,
  you have no choice other than to put the handlers in de code behind. You should, however, pass along control to the VM as soon as you can. If you
  use `MouseDown` to add new boids, have the V extract the coordinates from the parameters and then ask the VM to add a boid at that position.



In case of doubt, ask for clarifications during lab sessions or email. This page will be updated with the most frequently asked questions.
We advice you check this page regularly for new information.
