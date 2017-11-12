var olBaseMap = {
    mapCenter: [120.906712, 30.931338],
    mapZoom: 13,
    map: null,//地图变量
    initBaseMap: function () {
        var that = this;
        var tileMapLayer = new ol.layer.Tile({
            source: new ol.source.XYZ({
                url: 'http://119.23.128.14:8088/static/tilemaps/hqtgoogle/{z}/{x}/{-y}.png'
            }),
            preload: 5,
            minResolution: 0.17578125
        });
        that.map = new ol.Map({
            layers: [tileMapLayer],
            target: 'map',
            renderer: 'canvas',
            loadTilesWhileAnimating: true,
            loadTilesWhileInteracting: true,
            controls: ol.control.defaults().extend([new ol.control.ScaleLine()]),
            view: new ol.View({
                center: ol.proj.transform(that.mapCenter, 'EPSG:4326', 'EPSG:900913'),
                zoom: that.mapZoom,
                projection: "EPSG:900913",
                maxZoom: 19,
                minZoom: 9
            })
        });
        that.map.on('pointermove', function (evt) {
            var pointer = ol.proj.transform(evt.coordinate, 'EPSG:900913', 'EPSG:4326');
            document.getElementById('coord').innerHTML = pointer[0].toString() + ';' + pointer[1].toString();

        });
        return that.map;
    }
}
var olMeasure = {
    map: null,
    measureTooltipElement: null,
    measureTooltip: null,
    draw: null,
    sketch: null,
    source: null,
    vector: null,
    init: function (instanceMap) {
        var that = this;
        that.map = instanceMap;
        that.source = new ol.source.Vector();
        that.vector = new ol.layer.Vector({
            source: that.source,
            style: new ol.style.Style({
                fill: new ol.style.Fill({
                    color: 'rgba(255, 255, 255, 0.2)'
                }),
                stroke: new ol.style.Stroke({
                    color: 'blue',
                    width: 2
                })
            })
        });
        that.map.addLayer(that.vector);
        that.initBtn();
    },
    initBtn: function () {
        var that = this;
        $("#measure-panel").html('<div id="expander" style="float:left;background-color:#006699;height:30px;line-height:40px;" ><img src="../img/up.png" style="width:20px;height:20px;cursor:pointer;"/></div><div  id="tool-panel" style="float:left;"><label id="zoomIn" title="放大" style="float:left;width:30px;height:30px;background-color:white;background-image:url(../img/tool_ZoomIn.png);background-position:center;"></label><label id="zoomOut" title="缩小" style="float:left;width:30px;height:30px;background-color:white;background-image:url(../img/tool_ZoomOut.png);background-position:center;"></label> <label id="length" title="测距" style="float:left;width:30px;height:30px;background-color:white;background-image:url(../img/tool_Tgauge.png);background-position:center;"></label><label id="area" title="测面积" style="float:left;width:30px;height:30px;background-color:white; background-image:url(../img/tool_poly.png);background-position:center;"></label> <label id="eraser" title="退出测量" style="width:30px;height:30px;background-color:white; background-image:url(../img/tool_Tclear.png);background-position:center;"></label></div>');
        $("#measure-panel").draggable({
            containment: "parent"
        });
        $("#expander>img").css({ transform: "rotate(90deg)" });
        $("#zoomIn").click(function () {
            olMeasure.zoomIn();
        });
        $("#zoomOut").click(function () {
            olMeasure.zoomOut();
        });
        $("#length").click(function () {
            olMeasure.measureLength();
        });
        $("#eraser").click(function () {
            olMeasure.measureEndClear();
        });
        $("#area").click(function () {
            olMeasure.measureArea();
        });
        $("#expander").click(function () {
            $("#tool-panel").toggle("slide", {}, 500);
        });
        $("body").mousedown(function (event) {
            // right mouse button
            if (event.which == 3) {
                that.measureEndClear();
                this.oncontextmenu = function () { return false; }
            }
        });
        $("#tool-panel label").css("cursor", "pointer");
    },
    measureLength: function () {
        var that = this;
        if (that.draw != null) {
            that.map.removeInteraction(that.draw);
            that.draw != null;
        }
        that.addInteraction('LineString');
    },
    measureArea: function () {
        var that = this;
        if (that.draw != null) {
            that.map.removeInteraction(that.draw);
            that.draw != null;
        }
        that.addInteraction('Polygon');
    },
    measureEndClear: function () {
        var that = this;
        that.source.clear();
        $(".measure-static").remove();
        $(".measure-dynamic").remove();
        if (that.draw != null) {
            that.map.removeInteraction(that.draw);
            that.draw == null;
        }
    },
    zoomIn: function () {
        $(".ol-zoom-in").click(); return;
        var that = this;
        var view = that.map.getView();
        var curZoom = view.getZoom();
        if (curZoom < 18) {
            view.setZoom(curZoom + 1);
        }
    },
    zoomOut: function () {
        $(".ol-zoom-out").click(); return;
        var that = this;
        var view = that.map.getView();
        var curZoom = view.getZoom();
        if (curZoom > 1) {
            view.setZoom(curZoom - 1);
        }
    },
    formatLength: function (line) {
        var that = this;
        var wgs84Sphere = new ol.Sphere(6378137);
        var length = 0;//= Math.round(line.getLength() * 100) / 100;
        var coordinates = line.getCoordinates();
        var sourceProj = that.map.getView().getProjection();
        for (var i = 0, ii = coordinates.length - 1; i < ii; ++i) {
            var c1 = ol.proj.transform(coordinates[i], sourceProj, 'EPSG:4326');
            var c2 = ol.proj.transform(coordinates[i + 1], sourceProj, 'EPSG:4326');
            length += wgs84Sphere.haversineDistance(c1, c2);
        }
        var output;
        if (length > 100) {
            output = (Math.round(length / 1000 * 100) / 100) + ' ' + 'km';
        }
        else {
            output = (Math.round(length * 100) / 100) + ' ' + 'm';
        }
        var styleStr = "<div style='background-color:white;color:black;font-weight:bold;' class='measure-dynamic'>{0}<div>"
        return styleStr.format(output);
    },
    formatArea: function (polygon) {
        var area;//= polygon.getArea();
        var sourceProj = this.map.getView().getProjection();
        var geom = /** @type {ol.geom.Polygon} */(polygon.clone().transform(
            sourceProj, 'EPSG:4326'));
        var coordinates = geom.getLinearRing(0).getCoordinates();
        var wgs84Sphere = new ol.Sphere(6378137);
        area = Math.abs(wgs84Sphere.geodesicArea(coordinates));

        var resultMU = (Math.round(area * 100 * 0.0015) / 100);
        var output = resultMU + ' ' + '亩';
        if (resultMU > 10000) output = Math.round(resultMU / 10000) + ' ' + '万亩';
        var styleStr = "<div style='background-color:white;color:black;font-weight:bold;' class='measure-dynamic'>{0}<div>"
        return styleStr.format(output);
    },
    createMeasureTooltip: function () {
        var that = this;
        if (that.measureTooltipElement) {
            that.measureTooltipElement.parentNode.removeChild(that.measureTooltipElement);
        }
        that.measureTooltipElement = document.createElement("div");
        that.measureTooltip = new ol.Overlay({
            element: that.measureTooltipElement,//measureTooltipElement
            offset: [0, -15],
            positioning: 'bottom-center'
        });
        that.map.addOverlay(that.measureTooltip);
    },
    getStyle: function () {
        return new ol.style.Style({
            fill: new ol.style.Fill({
                color: 'rgba(255, 255, 255, 0.2)'
            }),
            stroke: new ol.style.Stroke({
                color: 'rgba(255, 0, 0, 0.5)',
                lineDash: [10, 10],
                width: 2
            }),
            image: new ol.style.Circle({
                radius: 3,
                stroke: new ol.style.Stroke({
                    color: 'rgba(255, 0, 0, 1)'
                }),
                fill: new ol.style.Fill({
                    color: 'rgba(255, 0, 0, 1)'
                })
            })
        });
    },
    addInteraction: function (type /*LineString or Polygon*/) {
        var that = this;
        that.draw = new ol.interaction.Draw({
            source: that.source,
            type: type,
            style: that.getStyle()
        });
        that.map.addInteraction(that.draw);
        that.createMeasureTooltip();
        var listener;
        that.draw.on('drawstart',
            function (evt) {
                that.sketch = evt.feature;
                var tooltipCoord = evt.coordinate;
                listener = that.sketch.getGeometry().on('change', function (evt) {
                    var geom = evt.target;
                    var output;
                    if (geom instanceof ol.geom.Polygon) {
                        output = that.formatArea(geom);
                        tooltipCoord = geom.getInteriorPoint().getCoordinates();
                    } else if (geom instanceof ol.geom.LineString) {
                        output = that.formatLength(geom);
                        tooltipCoord = geom.getLastCoordinate();
                    }
                    olMeasure.measureTooltipElement.innerHTML = output;
                    olMeasure.measureTooltip.setPosition(tooltipCoord);
                });
            }, this);
        that.draw.on("drawend", function () {
            that.measureTooltipElement.className = 'measure-static';
            that.measureTooltip.setOffset([0, -7]);
            sketch = null;
            that.measureTooltipElement = null;
            that.createMeasureTooltip();
            ol.Observable.unByKey(listener);
        }, this);
    }
};


String.prototype.format = function (args) {
    if (arguments.length > 0) {
        var result = this;
        if (arguments.length == 1 && typeof (args) == "object") {
            for (var key in args) {
                var reg = new RegExp("({" + key + "})", "g");
                result = result.replace(reg, args[key]);
            }
        }
        else {
            for (var i = 0; i < arguments.length; i++) {
                if (arguments[i] == undefined) {
                    return "";
                }
                else {
                    var reg = new RegExp("({[" + i + "]})", "g");
                    result = result.replace(reg, arguments[i]);
                }
            }
        }
        return result;
    }
    else {
        return this;
    }
}