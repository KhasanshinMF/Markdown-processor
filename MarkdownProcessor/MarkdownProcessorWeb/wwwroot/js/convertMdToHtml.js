function convertMarkdownToHtml() {
    var markdown = document.getElementById('markdown-content').value;

    fetch('/Document/ConvertToHtml', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(markdown)
    })
        .then(response => {
            if (!response.ok) {
                throw new Error('Ошибка при конвертации');
            }
            return response.text();
        })
        .then(text => {
            document.getElementById('html-content').textContent = text;
        })
        .catch(error => {
            console.error('Ошибка:', error);
            document.getElementById('html-content').textContent = 'Ошибка при конвертации: ' + error.message;
        });
}

function debounce(func, wait) {
    let timeout;
    return function(...args) {
        clearTimeout(timeout);
        timeout = setTimeout(() => func.apply(this, args), wait);
    };
}

document.getElementById('markdown-content').addEventListener('input', debounce(convertMarkdownToHtml, 300));

setTimeout(convertMarkdownToHtml(), 100);