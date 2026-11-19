export function getSvgPoint(svg, clientX, clientY) {
    const matrix = svg.getScreenCTM();
    if (!matrix) {
        throw new Error("Map SVG is not available for coordinate conversion.");
    }

    const point = svg.createSVGPoint();
    point.x = clientX;
    point.y = clientY;
    const mapPoint = point.matrixTransform(matrix.inverse());
    return { x: mapPoint.x, y: mapPoint.y };
}

export function capturePointer(element, pointerId) {
    element.setPointerCapture(pointerId);
}
