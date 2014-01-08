﻿package edu.stanford.stereostats{	import flash.display.DisplayObject;	import flash.display.MovieClip;	import flash.display.StageScaleMode;	import flash.events.*;	import flash.external.*;	import flash.net.URLRequest;	import flash.display.Loader;	import flash.events.Event;	import flash.events.ProgressEvent;	import flash.text.TextField;	import scaleform.clik.*;	import scaleform.clik.events.*;	import scaleform.clik.controls.*;	import scaleform.clik.data.*;	import flash.text.TextFormat;	public class MainMenu3D extends MovieClip	{		public var LeftMenu:MainMenu;		public var RightMenu:MainMenu;		public var GrabbableBG:MovieClip;		private var stereoController:StereoController;		public function MainMenu3D()		{			addEventListener(Event.ENTER_FRAME, configUI);		}		public function configUI(e:Event):void		{			removeEventListener(Event.ENTER_FRAME, configUI);			GrabbableBG.addEventListener(MouseEvent.MOUSE_DOWN, stageMouseDown);			stereoController = new StereoController(LeftMenu,RightMenu);			LeftMenu.init(stereoController);			RightMenu.init(stereoController);			ExternalInterface.call("OnRegisterSWFCallback", this);		}		public function stageMouseDown(e:Event):void		{			ExternalInterface.call("SetStageMouse", true);			stage.addEventListener(MouseEvent.MOUSE_UP, stageMouseUp);		}		public function stageMouseUp(e:Event):void		{			stereoController.stageMouseUp(e);			ExternalInterface.call("SetStageMouse", false);			stage.removeEventListener(MouseEvent.MOUSE_UP, stageMouseUp);		}	}}