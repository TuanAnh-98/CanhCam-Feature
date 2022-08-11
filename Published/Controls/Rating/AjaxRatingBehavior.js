Type.registerNamespace('gbAjaxControlToolkit'); gbAjaxControlToolkit.BehaviorBase = function(element) { gbAjaxControlToolkit.BehaviorBase.initializeBase(this, [element]); this._clientStateFieldID = null; this._pageRequestManager = null; this._partialUpdateBeginRequestHandler = null; this._partialUpdateEndRequestHandler = null; }
gbAjaxControlToolkit.BehaviorBase.prototype = { initialize: function() { gbAjaxControlToolkit.BehaviorBase.callBaseMethod(this, 'initialize'); }, dispose: function() {
    gbAjaxControlToolkit.BehaviorBase.callBaseMethod(this, 'dispose'); if (this._pageRequestManager) {
        if (this._partialUpdateBeginRequestHandler) { this._pageRequestManager.remove_beginRequest(this._partialUpdateBeginRequestHandler); this._partialUpdateBeginRequestHandler = null; }
        if (this._partialUpdateEndRequestHandler) { this._pageRequestManager.remove_endRequest(this._partialUpdateEndRequestHandler); this._partialUpdateEndRequestHandler = null; }
        this._pageRequestManager = null;
    } 
}, get_ClientStateFieldID: function() { return this._clientStateFieldID; }, set_ClientStateFieldID: function(value) { if (this._clientStateFieldID != value) { this._clientStateFieldID = value; this.raisePropertyChanged('ClientStateFieldID'); } }, get_ClientState: function() {
    if (this._clientStateFieldID) { var input = document.getElementById(this._clientStateFieldID); if (input) { return input.value; } }
    return null;
}, set_ClientState: function(value) { if (this._clientStateFieldID) { var input = document.getElementById(this._clientStateFieldID); if (input) { input.value = value; } } }, registerPartialUpdateEvents: function() { if (Sys && Sys.WebForms && Sys.WebForms.PageRequestManager) { this._pageRequestManager = Sys.WebForms.PageRequestManager.getInstance(); if (this._pageRequestManager) { this._partialUpdateBeginRequestHandler = Function.createDelegate(this, this._partialUpdateBeginRequest); this._pageRequestManager.add_beginRequest(this._partialUpdateBeginRequestHandler); this._partialUpdateEndRequestHandler = Function.createDelegate(this, this._partialUpdateEndRequest); this._pageRequestManager.add_endRequest(this._partialUpdateEndRequestHandler); } } }, _partialUpdateBeginRequest: function(sender, beginRequestEventArgs) { }, _partialUpdateEndRequest: function(sender, endRequestEventArgs) { } 
}
gbAjaxControlToolkit.BehaviorBase.registerClass('gbAjaxControlToolkit.BehaviorBase', Sys.UI.Behavior); gbAjaxControlToolkit.DynamicPopulateBehaviorBase = function(element) { gbAjaxControlToolkit.DynamicPopulateBehaviorBase.initializeBase(this, [element]); this._DynamicControlID = null; this._DynamicContextKey = null; this._DynamicServicePath = null; this._DynamicServiceMethod = null; this._cacheDynamicResults = false; this._dynamicPopulateBehavior = null; this._populatingHandler = null; this._populatedHandler = null; }
gbAjaxControlToolkit.DynamicPopulateBehaviorBase.prototype = { initialize: function() { gbAjaxControlToolkit.DynamicPopulateBehaviorBase.callBaseMethod(this, 'initialize'); this._populatingHandler = Function.createDelegate(this, this._onPopulating); this._populatedHandler = Function.createDelegate(this, this._onPopulated); }, dispose: function() {
    if (this._populatedHandler) {
        if (this._dynamicPopulateBehavior) { this._dynamicPopulateBehavior.remove_populated(this._populatedHandler); }
        this._populatedHandler = null;
    }
    if (this._populatingHandler) {
        if (this._dynamicPopulateBehavior) { this._dynamicPopulateBehavior.remove_populating(this._populatingHandler); }
        this._populatingHandler = null;
    }
    if (this._dynamicPopulateBehavior) { this._dynamicPopulateBehavior.dispose(); this._dynamicPopulateBehavior = null; }
    gbAjaxControlToolkit.DynamicPopulateBehaviorBase.callBaseMethod(this, 'dispose');
}, populate: function(contextKeyOverride) {
    if (this._dynamicPopulateBehavior && (this._dynamicPopulateBehavior.get_element() != $get(this._DynamicControlID))) { this._dynamicPopulateBehavior.dispose(); this._dynamicPopulateBehavior = null; }
    if (!this._dynamicPopulateBehavior && this._DynamicControlID && this._DynamicServiceMethod) { this._dynamicPopulateBehavior = $create(gbAjaxControlToolkit.DynamicPopulateBehavior, { "id": this.get_id() + "_DynamicPopulateBehavior", "ContextKey": this._DynamicContextKey, "ServicePath": this._DynamicServicePath, "ServiceMethod": this._DynamicServiceMethod, "cacheDynamicResults": this._cacheDynamicResults }, null, null, $get(this._DynamicControlID)); this._dynamicPopulateBehavior.add_populating(this._populatingHandler); this._dynamicPopulateBehavior.add_populated(this._populatedHandler); }
    if (this._dynamicPopulateBehavior) { this._dynamicPopulateBehavior.populate(contextKeyOverride ? contextKeyOverride : this._DynamicContextKey); } 
}, _onPopulating: function(sender, eventArgs) { this.raisePopulating(eventArgs); }, _onPopulated: function(sender, eventArgs) { this.raisePopulated(eventArgs); }, get_dynamicControlID: function() { return this._DynamicControlID; }, get_DynamicControlID: this.get_dynamicControlID, set_dynamicControlID: function(value) { if (this._DynamicControlID != value) { this._DynamicControlID = value; this.raisePropertyChanged('dynamicControlID'); this.raisePropertyChanged('DynamicControlID'); } }, set_DynamicControlID: this.set_dynamicControlID, get_dynamicContextKey: function() { return this._DynamicContextKey; }, get_DynamicContextKey: this.get_dynamicContextKey, set_dynamicContextKey: function(value) { if (this._DynamicContextKey != value) { this._DynamicContextKey = value; this.raisePropertyChanged('dynamicContextKey'); this.raisePropertyChanged('DynamicContextKey'); } }, set_DynamicContextKey: this.set_dynamicContextKey, get_dynamicServicePath: function() { return this._DynamicServicePath; }, get_DynamicServicePath: this.get_dynamicServicePath, set_dynamicServicePath: function(value) { if (this._DynamicServicePath != value) { this._DynamicServicePath = value; this.raisePropertyChanged('dynamicServicePath'); this.raisePropertyChanged('DynamicServicePath'); } }, set_DynamicServicePath: this.set_dynamicServicePath, get_dynamicServiceMethod: function() { return this._DynamicServiceMethod; }, get_DynamicServiceMethod: this.get_dynamicServiceMethod, set_dynamicServiceMethod: function(value) { if (this._DynamicServiceMethod != value) { this._DynamicServiceMethod = value; this.raisePropertyChanged('dynamicServiceMethod'); this.raisePropertyChanged('DynamicServiceMethod'); } }, set_DynamicServiceMethod: this.set_dynamicServiceMethod, get_cacheDynamicResults: function() { return this._cacheDynamicResults; }, set_cacheDynamicResults: function(value) { if (this._cacheDynamicResults != value) { this._cacheDynamicResults = value; this.raisePropertyChanged('cacheDynamicResults'); } }, add_populated: function(handler) { this.get_events().addHandler("populated", handler); }, remove_populated: function(handler) { this.get_events().removeHandler("populated", handler); }, raisePopulated: function(arg) { var handler = this.get_events().getHandler("populated"); if (handler) handler(this, arg); }, add_populating: function(handler) { this.get_events().addHandler('populating', handler); }, remove_populating: function(handler) { this.get_events().removeHandler('populating', handler); }, raisePopulating: function(eventArgs) { var handler = this.get_events().getHandler('populating'); if (handler) { handler(this, eventArgs); } } 
}
gbAjaxControlToolkit.DynamicPopulateBehaviorBase.registerClass('gbAjaxControlToolkit.DynamicPopulateBehaviorBase', gbAjaxControlToolkit.BehaviorBase); gbAjaxControlToolkit.ControlBase = function(element) { gbAjaxControlToolkit.ControlBase.initializeBase(this, [element]); this._clientStateField = null; this._callbackTarget = null; this._onsubmit$delegate = Function.createDelegate(this, this._onsubmit); this._oncomplete$delegate = Function.createDelegate(this, this._oncomplete); this._onerror$delegate = Function.createDelegate(this, this._onerror); }
gbAjaxControlToolkit.ControlBase.__doPostBack = function(eventTarget, eventArgument) {
    if (!Sys.WebForms.PageRequestManager.getInstance().get_isInAsyncPostBack()) { for (var i = 0; i < gbAjaxControlToolkit.ControlBase.onsubmitCollection.length; i++) { gbAjaxControlToolkit.ControlBase.onsubmitCollection[i](); } }
    Function.createDelegate(window, gbAjaxControlToolkit.ControlBase.__doPostBackSaved)(eventTarget, eventArgument);
}
gbAjaxControlToolkit.ControlBase.prototype = { initialize: function() {
    gbAjaxControlToolkit.ControlBase.callBaseMethod(this, "initialize"); if (this._clientStateField) { this.loadClientState(this._clientStateField.value); }
    if (typeof (Sys.WebForms) !== "undefined" && typeof (Sys.WebForms.PageRequestManager) !== "undefined") {
        Array.add(Sys.WebForms.PageRequestManager.getInstance()._onSubmitStatements, this._onsubmit$delegate); if (gbAjaxControlToolkit.ControlBase.__doPostBackSaved == null || typeof gbAjaxControlToolkit.ControlBase.__doPostBackSaved == "undefined") { gbAjaxControlToolkit.ControlBase.__doPostBackSaved = window.__doPostBack; window.__doPostBack = gbAjaxControlToolkit.ControlBase.__doPostBack; gbAjaxControlToolkit.ControlBase.onsubmitCollection = new Array(); }
        Array.add(gbAjaxControlToolkit.ControlBase.onsubmitCollection, this._onsubmit$delegate);
    } else { $addHandler(document.forms[0], "submit", this._onsubmit$delegate); } 
}, dispose: function() {
    if (typeof (Sys.WebForms) !== "undefined" && typeof (Sys.WebForms.PageRequestManager) !== "undefined") { Array.remove(gbAjaxControlToolkit.ControlBase.onsubmitCollection, this._onsubmit$delegate); Array.remove(Sys.WebForms.PageRequestManager.getInstance()._onSubmitStatements, this._onsubmit$delegate); } else { $removeHandler(document.forms[0], "submit", this._onsubmit$delegate); }
    gbAjaxControlToolkit.ControlBase.callBaseMethod(this, "dispose");
}, findElement: function(id) { return $get(this.get_id() + '_' + id.split(':').join('_')); }, get_clientStateField: function() { return this._clientStateField; }, set_clientStateField: function(value) { if (this.get_isInitialized()) throw Error.invalidOperation("ExtenderBase_CannotSetClientStateField"); if (this._clientStateField != value) { this._clientStateField = value; this.raisePropertyChanged('clientStateField'); } }, loadClientState: function(value) { }, saveClientState: function() { return null; }, _invoke: function(name, args, cb) {
    if (!this._callbackTarget) { throw Error.invalidOperation("ExtenderBase_ControlNotRegisteredForCallbacks"); }
    if (typeof (WebForm_DoCallback) === "undefined") { throw Error.invalidOperation("ExtenderBase_PageNotRegisteredForCallbacks"); }
    var ar = []; for (var i = 0; i < args.length; i++)
        ar[i] = args[i]; var clientState = this.saveClientState(); if (clientState != null && !String.isInstanceOfType(clientState)) { throw Error.invalidOperation("ExtenderBase_InvalidClientStateType"); }
    var payload = Sys.Serialization.JavaScriptSerializer.serialize({ name: name, args: ar, state: this.saveClientState() }); WebForm_DoCallback(this._callbackTarget, payload, this._oncomplete$delegate, cb, this._onerror$delegate, true);
}, _oncomplete: function(result, context) {
    result = Sys.Serialization.JavaScriptSerializer.deserialize(result); if (result.error) { throw Error.create(result.error); }
    this.loadClientState(result.state); context(result.result);
}, _onerror: function(message, context) { throw Error.create(message); }, _onsubmit: function() {
    if (this._clientStateField) { this._clientStateField.value = this.saveClientState(); }
    return true;
} 
}
gbAjaxControlToolkit.ControlBase.registerClass("gbAjaxControlToolkit.ControlBase", Sys.UI.Control); gbAjaxControlToolkit.AjaxRatingBehavior = function(element) { gbAjaxControlToolkit.AjaxRatingBehavior.initializeBase(this, [element]); this._starCssClass = null; this._filledStarCssClass = null; this._emptyStarCssClass = null; this._waitingStarCssClass = null; this._readOnly = false; this._ratingValue = 0; this._currentRating = 0; this._maxRatingValue = 5; this._tag = ""; this._ratingDirection = 0; this._stars = null; this._callbackID = null; this._mouseOutHandler = Function.createDelegate(this, this._onMouseOut); this._starClickHandler = Function.createDelegate(this, this._onStarClick); this._starMouseOverHandler = Function.createDelegate(this, this._onStarMouseOver); this._keyDownHandler = Function.createDelegate(this, this._onKeyDownBack); this._autoPostBack = false; this._jsonUrl = ""; this._contentId = ""; this._totalVotesElementId = ""; this._commentsEnabled = false; }
gbAjaxControlToolkit.AjaxRatingBehavior.prototype = { initialize: function() {
    gbAjaxControlToolkit.AjaxRatingBehavior.callBaseMethod(this, 'initialize'); var elt = this.get_element(); this._stars = []; for (var i = 1; i <= this._maxRatingValue; i++) { starElement = $get(elt.id + '_Star_' + i); starElement.value = i; Array.add(this._stars, starElement); $addHandler(starElement, 'click', this._starClickHandler); $addHandler(starElement, 'mouseover', this._starMouseOverHandler); }
    $addHandler(elt, 'mouseout', this._mouseOutHandler); $addHandler(elt, "keydown", this._keyDownHandler); this._update();
}, dispose: function() {
    var elt = this.get_element(); if (this._stars) {
        for (var i = 0; i < this._stars.length; i++) { var starElement = this._stars[i]; $removeHandler(starElement, 'click', this._starClickHandler); $removeHandler(starElement, 'mouseover', this._starMouseOverHandler); }
        this._stars = null;
    }
    $removeHandler(elt, 'mouseout', this._mouseOutHandler); $removeHandler(elt, "keydown", this._keyDownHandler); gbAjaxControlToolkit.AjaxRatingBehavior.callBaseMethod(this, 'dispose');
}, _onError: function(message, context) { alert(String.format("callback error", message)); }, _receiveServerData: function(arg, context) { context._waitingMode(false); context.raiseEndClientCallback(arg); }, _onMouseOut: function(e) {
    if (this._readOnly) { return; }
    this._currentRating = this._ratingValue; this._update(); this.raiseMouseOut(this._currentRating);
}, _onStarClick: function(e) {
    if (this._readOnly) { return; }
    this.set_Rating(this._currentRating);
}, _onStarMouseOver: function(e) {
    if (this._readOnly) { return; }
    if (this._ratingDirection == 0) { this._currentRating = e.target.value; } else { this._currentRating = this._maxRatingValue + 1 - e.target.value; }
    this._update(); this.raiseMouseOver(this._currentRating);
}, _onKeyDownBack: function(ev) {
    if (this._readOnly) { return; }
    var k = ev.keyCode ? ev.keyCode : ev.rawEvent.keyCode; if ((k == Sys.UI.Key.right) || (k == Sys.UI.Key.up)) { this._currentRating = Math.min(this._currentRating + 1, this._maxRatingValue); this.set_Rating(this._currentRating); ev.preventDefault(); ev.stopPropagation(); } else if ((k == Sys.UI.Key.left) || (k == Sys.UI.Key.down)) { this._currentRating = Math.max(this._currentRating - 1, 1); this.set_Rating(this._currentRating); ev.preventDefault(); ev.stopPropagation(); } 
}, _waitingMode: function(activated) {
    for (var i = 0; i < this._maxRatingValue; i++) {
        var starElement; if (this._ratingDirection == 0) { starElement = this._stars[i]; } else { starElement = this._stars[this._maxRatingValue - i - 1]; }
        if (this._currentRating > i) { if (activated) { Sys.UI.DomElement.removeCssClass(starElement, this._filledStarCssClass); Sys.UI.DomElement.addCssClass(starElement, this._waitingStarCssClass); } else { Sys.UI.DomElement.removeCssClass(starElement, this._waitingStarCssClass); Sys.UI.DomElement.addCssClass(starElement, this._filledStarCssClass); } } else { Sys.UI.DomElement.removeCssClass(starElement, this._waitingStarCssClass); Sys.UI.DomElement.removeCssClass(starElement, this._filledStarCssClass); Sys.UI.DomElement.addCssClass(starElement, this._emptyStarCssClass); } 
    } 
}, _update: function() {
    var elt = this.get_element(); $get(elt.id + "_A").title = this._currentRating; for (var i = 0; i < this._maxRatingValue; i++) {
        var starElement; if (this._ratingDirection == 0) { starElement = this._stars[i]; } else { starElement = this._stars[this._maxRatingValue - i - 1]; }
        if (this._currentRating > i) { Sys.UI.DomElement.removeCssClass(starElement, this._emptyStarCssClass); Sys.UI.DomElement.addCssClass(starElement, this._filledStarCssClass); }
        else { Sys.UI.DomElement.removeCssClass(starElement, this._filledStarCssClass); Sys.UI.DomElement.addCssClass(starElement, this._emptyStarCssClass); } 
    } 
}, add_Rated: function(handler) { this.get_events().addHandler("Rated", handler); }, remove_Rated: function(handler) { this.get_events().removeHandler("Rated", handler); }, raiseRated: function(rating) { var handler = this.get_events().getHandler("Rated"); if (handler) { handler(this, new gbAjaxControlToolkit.AjaxRatingEventArgs(rating)); } }, add_MouseOver: function(handler) { this.get_events().addHandler("MouseOver", handler); }, remove_MouseOver: function(handler) { this.get_events().removeHandler("MouseOver", handler); }, raiseMouseOver: function(rating_tmp) { var handler = this.get_events().getHandler("MouseOver"); if (handler) { handler(this, new gbAjaxControlToolkit.AjaxRatingEventArgs(rating_tmp)); } }, add_MouseOut: function(handler) { this.get_events().addHandler("MouseOut", handler); }, remove_MouseOut: function(handler) { this.get_events().removeHandler("MouseOut", handler); }, raiseMouseOut: function(rating_old) { var handler = this.get_events().getHandler("MouseOut"); if (handler) { handler(this, new gbAjaxControlToolkit.AjaxRatingEventArgs(rating_old)); } }, add_EndClientCallback: function(handler) { this.get_events().addHandler("EndClientCallback", handler); }, remove_EndClientCallback: function(handler) { this.get_events().removeHandler("EndClientCallback", handler); }, raiseEndClientCallback: function(result) { var handler = this.get_events().getHandler("EndClientCallback"); if (handler) { handler(this, new gbAjaxControlToolkit.AjaxRatingCallbackResultEventArgs(result)); } }, get_AutoPostBack: function() { return this._autoPostBack; }, set_AutoPostBack: function(value) { this._autoPostBack = value; }, get_Stars: function() { return this._stars; }, get_Tag: function() { return this._tag; }, set_Tag: function(value) { if (this._tag != value) { this._tag = value; this.raisePropertyChanged('Tag'); } }, get_JsonUrl: function() { return this._jsonUrl; }, set_JsonUrl: function(value) { if (this._jsonUrl != value) { this._jsonUrl = value; this.raisePropertyChanged('JsonUrl'); } }, get_ContentId: function() { return this._contentId; }, set_ContentId: function(value) { if (this._contentId != value) { this._contentId = value; this.raisePropertyChanged('ContentId'); } }, get_TotalVotesElementId: function() { return this._totalVotesElementId; }, set_TotalVotesElementId: function(value) { if (this._totalVotesElementId != value) { this._totalVotesElementId = value; this.raisePropertyChanged('TotalVotesElementId'); } }, get_CallbackID: function() { return this._callbackID; }, set_CallbackID: function(value) { this._callbackID = value; }, get_RatingDirection: function() { return this._ratingDirection; }, set_RatingDirection: function(value) {
    if (this._ratingDirection != value) {
        this._ratingDirection = value; if (this.get_isInitialized()) { this._update(); }
        this.raisePropertyChanged('RatingDirection');
    } 
}, get_EmptyStarCssClass: function() { return this._emptyStarCssClass; }, set_EmptyStarCssClass: function(value) { if (this._emptyStarCssClass != value) { this._emptyStarCssClass = value; this.raisePropertyChanged('EmptyStarCssClass'); } }, get_FilledStarCssClass: function() { return this._filledStarCssClass; }, set_FilledStarCssClass: function(value) { if (this._filledStarCssClass != value) { this._filledStarCssClass = value; this.raisePropertyChanged('FilledStarCssClass'); } }, get_WaitingStarCssClass: function() { return this._waitingStarCssClass; }, set_WaitingStarCssClass: function(value) { if (this._waitingStarCssClass != value) { this._waitingStarCssClass = value; this.raisePropertyChanged('WaitingStarCssClass'); } }, get_Rating: function() {
    this._ratingValue = gbAjaxControlToolkit.AjaxRatingBehavior.callBaseMethod(this, 'get_ClientState'); if (this._ratingValue == '')
        this._ratingValue = null; return this._ratingValue;
}, set_Rating: function(value) {
    this._ratingValue = value; this._currentRating = value; if (this.get_isInitialized()) {
        if ((value < 0) || (value > this._maxRatingValue)) { return; }
        this._update(); gbAjaxControlToolkit.AjaxRatingBehavior.callBaseMethod(this, 'set_ClientState', [this._ratingValue]); this.raisePropertyChanged('Rating'); this.raiseRated(this._currentRating); if (this._commentsEnabled) { return; }
        this._waitingMode(true); var args = this._currentRating + ";" + this._tag; var id = this._callbackID; var result = null; var votes = null; if ((this._jsonUrl.length > 0) && (this._contentId.length == 36)) {
            var ratingData = "cid=" + this._contentId + "&r=" + this._ratingValue; $.ajax({ type: "POST", async: false, processData: false, url: this._jsonUrl, data: ratingData, contentType: "text/javascript", dataType: "json", success: function(data) { result = data.avg; votes = data.votes; } }); if ((this._totalVotesElementId.length > 0) && (votes != null)) { $('#' + this._totalVotesElementId).html(votes); }
            this._waitingMode(false); this._currentRating = result; this._ratingValue = result; this._update();
        }
        else if (this._autoPostBack) { __doPostBack(id, args); }
        else { WebForm_DoCallback(id, args, this._receiveServerData, this, this._onError, true) } 
    } 
}, get_MaxRating: function() { return this._maxRatingValue; }, set_MaxRating: function(value) { if (this._maxRatingValue != value) { this._maxRatingValue = value; this.raisePropertyChanged('MaxRating'); } }, get_ReadOnly: function() { return this._readOnly; }, set_ReadOnly: function(value) { if (this._readOnly != value) { this._readOnly = value; this.raisePropertyChanged('ReadOnly'); } }, get_CommentsEnabled: function() { return this._commentsEnabled; }, set_CommentsEnabled: function(value) { if (this._commentsEnabled != value) { this._commentsEnabled = value; this.raisePropertyChanged('CommentsEnabled'); } }, get_StarCssClass: function() { return this._starCssClass; }, set_StarCssClass: function(value) { if (this._starCssClass != value) { this._starCssClass = value; this.raisePropertyChanged('StarCssClass'); } } 
}
gbAjaxControlToolkit.AjaxRatingBehavior.registerClass('gbAjaxControlToolkit.AjaxRatingBehavior', gbAjaxControlToolkit.BehaviorBase); gbAjaxControlToolkit.AjaxRatingEventArgs = function(rating) { gbAjaxControlToolkit.AjaxRatingEventArgs.initializeBase(this); this._rating = rating; }
gbAjaxControlToolkit.AjaxRatingEventArgs.prototype = { get_Rating: function() { return this._rating; } }
gbAjaxControlToolkit.AjaxRatingEventArgs.registerClass('gbAjaxControlToolkit.AjaxRatingEventArgs', Sys.EventArgs); gbAjaxControlToolkit.AjaxRatingCallbackResultEventArgs = function(result) { gbAjaxControlToolkit.AjaxRatingCallbackResultEventArgs.initializeBase(this); this._result = result; }
gbAjaxControlToolkit.AjaxRatingCallbackResultEventArgs.prototype = { get_CallbackResult: function() { return this._result; } }
gbAjaxControlToolkit.AjaxRatingCallbackResultEventArgs.registerClass('gbAjaxControlToolkit.AjaxRatingCallbackResultEventArgs', Sys.EventArgs);
