document.getElementById('generate-link-btn').addEventListener('click', async function () {
    const documentId = document.querySelector('input[name="Id"]').value;

    try {
        const response = await fetch(`/Document/GenerateShareLink?id=${documentId}`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            }
        });

        if (response.ok) {
            const data = await response.json();
            const linkSection = document.getElementById('generated-link-section');
            const linkElement = document.getElementById('generated-link');

            linkElement.href = data.link;
            linkElement.textContent = data.link;
            linkSection.style.display = 'block';
        } else {
            alert('Ошибка при генерации ссылки');
        }
    } catch (error) {
        console.error('Ошибка:', error);
    }
});