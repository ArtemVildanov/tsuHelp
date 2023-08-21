const openDealModalButton = document.getElementById('openDealModal');
const dealModal = document.getElementById('dealModal');
const closeDealModalButton = document.getElementById('closeDealModal');

openDealModalButton.addEventListener('click', () => {
    dealModal.style.display = 'block';
});

// При нажатии на крестик или за пределы модального окна, модальное окно закрывается
closeDealModalButton.addEventListener('click', () => {
    dealModal.style.display = 'none';
});

window.addEventListener('click', (event) => {
    if (event.target === dealModal) {
        dealModal.style.display = 'none';
    }
});