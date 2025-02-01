document.getElementById('confirm-add-user-btn').addEventListener('click', async function () {
    const documentId = document.querySelector('input[name="Id"]').value;
    const username = document.getElementById('username').value;
    const accessLevel = document.getElementById('accessLevel').value;

    try {
        const response = await fetch(`/Document/AddUserToDocument?id=${documentId}&username=${username}&accessLevel=${accessLevel}`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            }
        });

        if (response.ok) {
            alert('Пользователь успешно добавлен');
            $('#addUserModal').modal('hide');
        } else {
            alert('Ошибка при добавлении пользователя');
        }
    } catch (error) {
        console.error('Ошибка:', error);
    }
});