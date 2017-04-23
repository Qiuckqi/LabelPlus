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
    try{
      ref1.putProperty(cTID('Chnl'), sTID("selection"));
      desc1.putReference(cTID('null'), ref1);
      desc1.putEnumerated(cTID('T   '), cTID('Ordn'), cTID('None'));
      executeAction(sTID('set'), desc1, DialogModes.NO);
    }
    catch(e){}
};    

// ����ѡ��
MyAction.selectInverse = function() { 
    try{
    executeAction(cTID('Invs'), undefined, DialogModes.NO);
    }
    catch(e){}
};  

// ѡ������(����)
MyAction.selectContract = function(pxl) { 
    var desc1 = new ActionDescriptor();
    try{  
      desc1.putUnitDouble(cTID('By  '), cTID('#Pxl'), pxl);
      executeAction(cTID('Cntc'), desc1, DialogModes.NO);
    }
    catch(e){}
};

// ѡ����չ(����)
MyAction.selectExpand = function(pxl) {  
    var desc1 = new ActionDescriptor();
    try{
      desc1.putUnitDouble(cTID('By  '), cTID('#Pxl'), pxl);
      executeAction(cTID('Expn'), desc1, DialogModes.NO);
    }
    catch(e){}
};  

// ħ��(x, y, �ݲ�, ��������ͼ��, �����, ��ѡ����ʽ�ַ���)
// ��ѡ����ʽ�ַ��� ����Ϊ:
// 'setd' �½�����
// 'addTo'    ���
// 'subtractFrom' �Ƴ�
// 'interfaceWhite'  ����
MyAction.magicWand = function(x, y, tolerance, merged, antiAlias, newAreaModeStr) {
    try{
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
    }
    catch(e){}
  
};

// �½�ͼ��
MyAction.newLyr = function() {     
    try{
        var desc1 = new ActionDescriptor();
        var ref1 = new ActionReference();
        ref1.putClass(cTID('Lyr '));
        desc1.putReference(cTID('null'), ref1);
        executeAction(cTID('Mk  '), desc1, DialogModes.NO);   
    }
    catch(e){}
}
// ɾ����ǰͼ��
MyAction.delLyr = function() {
    try{
        var desc1 = new ActionDescriptor();
        var ref1 = new ActionReference();
        ref1.putEnumerated(cTID('Lyr '), cTID('Ordn'), cTID('Trgt'));
        desc1.putReference(cTID('null'), ref1);
        executeAction(cTID('Dlt '), desc1, DialogModes.NO);        
    }
    catch(e){}
}

// ���(ʹ��ʲô���, ͸����)
// use������:
// 'FrgC' ǰ��ɫ
// 'BckC' ����ɫ
// 'Blck' ��ɫ
// 'Gry ' ��ɫ  
// 'Wht ' ��ɫ
MyAction.fill = function(use, opct) { 
    try{
        var desc1 = new ActionDescriptor();
        desc1.putEnumerated(cTID('Usng'), cTID('FlCn'), cTID(use));
        desc1.putUnitDouble(cTID('Opct'), cTID('#Prc'), opct);
        desc1.putEnumerated(cTID('Md  '), cTID('BlnM'), cTID('Nrml'));
        executeAction(cTID('Fl  '), desc1, DialogModes.NO);
    }
    catch(e){}
};

// �Ի���Ϳ�׶���(��ǩ����, ͼ����, ͼ��߶�, ħ���ݲ�, ������������)
MyAction.lp_dialogClear = function(labelArr, imgWidht, imgHeight, tolerance, contract) {

    // �������
    if (labelArr.length == 0) {
      return;
    }

    MyAction.selectNone();
    
    // ��λת��
    imgWidht.convert("px");
    imgHeight.convert("px");
    
    // ѡ�����п��ڵĿհ�����
    for(var i=0; i<labelArr.length; i++){
        var x = (labelArr[i].x) * imgWidht;
        var y = (labelArr[i].y) * imgHeight;
        
        MyAction.magicWand(x, y, tolerance, true, true, 'addTo');
    }     
    
    // ���½��ĸ���ͼ���������հ�����
    MyAction.newLyr();
    MyAction.fill('Blck', 100);    
    
    // �ĸ���ʹ��ħ�� �ٷ�ѡ ����������
    MyAction.selectNone();
    MyAction.magicWand(0                , 0                  , tolerance, false, true, 'addTo');
    MyAction.magicWand(imgWidht-1 , 0                  , tolerance, false, true, 'addTo');
    MyAction.magicWand(0                , imgHeight-1  , tolerance, false, true, 'addTo');
    MyAction.magicWand(imgWidht-1 , imgHeight-1  , tolerance, false, true, 'addTo');
    MyAction.selectInverse();
    MyAction.selectContract(contract);
    
    // ɾ������ͼ�� ����Ϳ��ͼ�� ����䱳��ɫ
    MyAction.delLyr();
    MyAction.newLyr();
    MyAction.fill('BckC', 100);
    
    MyAction.selectNone();
    
};

// TEST
//MyAction.lp_dialogClear(0, 1715, 2500, 16, 1);

//MyAction.selectNone();
//MyAction.selectInverse();
//MyAction.selectContract(5);
//MyAction.selectExpand(5);
//MyAction.magicWand(); // x, y, tolerance, merged, antiAlias, newAreaModeStr
//MyAction.newLyr()
//MyAction.delLyr();
//MyAction.fill('Blck', 50);

"my_action.js";
