//
//   LabelPlus_Ps_Script.jsx
//   This is a Input Text Tool for LabelPlus Text File.
// 
// Copyright 2015, Noodlefighter
// Released under GPL License.
//
// License: http://noodlefighter.com/label_plus/license
//
//@show include
//@include "my_include.js"

//
// ������
// 
MyAction = function() { }

// ȡ��ѡ��
MyAction.selectNone = function() {
  var desc1 = new ActionDescriptor();
  var ref1 = new ActionReference();
  ref1.putProperty(cTID('Chnl'), sTID("selection"));
  desc1.putReference(cTID('null'), ref1);
  desc1.putEnumerated(cTID('T   '), cTID('Ordn'), cTID('None'));
  executeAction(sTID('set'), desc1, DialogModes.NO);
};    

// ����ѡ��
MyAction.selectInverse = function() { 
  executeAction(cTID('Invs'), undefined, DialogModes.NO);
};  

// ѡ������(����)
MyAction.selectContract = function(pxl) { 
  var desc1 = new ActionDescriptor();
  desc1.putUnitDouble(cTID('By  '), cTID('#Pxl'), pxl);
  executeAction(cTID('Cntc'), desc1, DialogModes.NO);
};

// ѡ����չ(����)
MyAction.selectExpand = function(pxl) {  
  var desc1 = new ActionDescriptor();
  desc1.putUnitDouble(cTID('By  '), cTID('#Pxl'), pxl);
  executeAction(cTID('Expn'), desc1, DialogModes.NO);
};  

// ħ��(x, y, �ݲ�, ��������ͼ��, �����, ��ѡ����ʽ�ַ���)
// ��ѡ����ʽ�ַ��� ����Ϊ:
// 'setd' �½�����
// 'addTo'    ���
// 'subtractFrom' �Ƴ�
// 'interfaceWhite'  ����
MyAction.magicWand = function(x, y, tolerance, merged, antiAlias, newAreaModeStr) {
  if(x == undefined || y == undefined){
    x = 0;
    y = 0;      
  }
  if(tolerance == undefined) 
      tolerance = 32;
  if(merged == undefined) 
      merged = false;
  if(antiAlias == undefined) 
      antiAlias = true;        
  if(newAreaModeStr == undefined || newAreaModeStr == '')
      newAreaModeStr = 'setd';
     
  var desc1 = new ActionDescriptor();
  var ref1 = new ActionReference();
  ref1.putProperty(cTID('Chnl'), sTID("selection"));
  desc1.putReference(cTID('null'), ref1);
  var desc2 = new ActionDescriptor();
  desc2.putUnitDouble(cTID('Hrzn'), cTID('#Pxl'), x);
  desc2.putUnitDouble(cTID('Vrtc'), cTID('#Pxl'), y);
  desc1.putObject(cTID('T   '), cTID('Pnt '), desc2);
  desc1.putInteger(cTID('Tlrn'), tolerance);
  desc1.putBoolean(cTID('Mrgd'), merged);
  desc1.putBoolean(cTID('AntA'), antiAlias);
  executeAction(sTID(newAreaModeStr), desc1, DialogModes.NO);
};

// �½�ͼ��
MyAction.newLyr = function() {     
  var desc1 = new ActionDescriptor();
  var ref1 = new ActionReference();
  ref1.putClass(cTID('Lyr '));
  desc1.putReference(cTID('null'), ref1);
  executeAction(cTID('Mk  '), desc1, DialogModes.NO);        
}
// ɾ����ǰͼ��
MyAction.delLyr = function() {
  var desc1 = new ActionDescriptor();
  var ref1 = new ActionReference();
  ref1.putEnumerated(cTID('Lyr '), cTID('Ordn'), cTID('Trgt'));
  desc1.putReference(cTID('null'), ref1);
  executeAction(cTID('Dlt '), desc1, DialogModes.NO);        
}

// ���(ʹ��ʲô���, ͸����)
// use������:
// 'FrgC' ǰ��ɫ
// 'BckC' ����ɫ
// 'Blck' ��ɫ
// 'Gry ' ��ɫ  
// 'Wht ' ��ɫ
MyAction.fill = function(use, opct) { 
  var desc1 = new ActionDescriptor();
  desc1.putEnumerated(cTID('Usng'), cTID('FlCn'), cTID(use));
  desc1.putUnitDouble(cTID('Opct'), cTID('#Prc'), opct);
  desc1.putEnumerated(cTID('Md  '), cTID('BlnM'), cTID('Nrml'));
  executeAction(cTID('Fl  '), desc1, DialogModes.NO);
};

// TEST
MyAction.selectNone();
//MyAction.fill('Blck', 50);


"my_action.js";
