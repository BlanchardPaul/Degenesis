window.readImageDimensions = (streamRef) => {
    return streamRef.stream()
        .then(stream => new Response(stream).arrayBuffer())
        .then(buffer => {
            return new Promise((resolve, reject) => {
                const blob = new Blob([buffer]);
                const url = URL.createObjectURL(blob);
                const img = new Image();
                img.onload = () => {
                    resolve({ width: img.width, height: img.height });
                    URL.revokeObjectURL(url);
                };
                img.onerror = () => {
                    resolve({ width: 0, height: 0 });
                    URL.revokeObjectURL(url);
                };
                img.src = url;
            });
        });
};