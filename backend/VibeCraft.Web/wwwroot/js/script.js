const galleryItems = document.querySelectorAll('.gallery-item');
const modal = document.getElementById("galleryModal");
const modalImg = document.getElementById("modalImg");
const modalCaption = document.getElementById("modalCaption");

let currentGallery = [];
let currentIndex = 0;

function openModal(itemIndex, startIndex = 0) {
  const item = galleryItems[itemIndex];
  if (!item) return;
  const imgs = item.querySelectorAll('img');
  currentGallery = Array.from(imgs).map(img => ({ src: img.src, alt: img.alt }));
  if (!currentGallery.length) return;
  currentIndex = Math.min(Math.max(0, startIndex), currentGallery.length - 1);
  modalCaption.innerText = item.querySelector('.label')?.innerText || '';
  updateModal();
  modal.style.display = 'block';
}

function updateModal() {
  if (!currentGallery.length) return;
  modalImg.src = currentGallery[currentIndex].src;
  modalImg.alt = currentGallery[currentIndex].alt || '';
}

function closeModal() {
  modal.style.display = 'none';
}

function changeImage(step) {
  if (!currentGallery.length) return;
  currentIndex = (currentIndex + step + currentGallery.length) % currentGallery.length;
  updateModal();
}

window.onclick = (e) => { if (e.target == modal) closeModal(); };

document.addEventListener('keydown', (e) => {
  if (modal.style.display === 'block') {
    if (e.key === 'ArrowLeft') changeImage(-1);
    if (e.key === 'ArrowRight') changeImage(1);
    if (e.key === 'Escape') closeModal();
  }
});