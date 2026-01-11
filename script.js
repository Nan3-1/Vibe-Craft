let currentIndex = 0;
const images = document.querySelectorAll('.gallery-item img');
const labels = document.querySelectorAll('.label');
const modal = document.getElementById("galleryModal");
const modalImg = document.getElementById("modalImg");
const modalCaption = document.getElementById("modalCaption");

function openModal(index) {
  currentIndex = index;
  updateModal();
  modal.style.display = "block";
}

function closeModal() {
  modal.style.display = "none";
}

function changeImage(step) {
  currentIndex += step;
  // Loop back logic
  if (currentIndex >= images.length) currentIndex = 0;
  if (currentIndex < 0) currentIndex = images.length - 1;
  updateModal();
}

function updateModal() {
  modalImg.src = images[currentIndex].src;
  modalCaption.innerHTML = labels[currentIndex].innerText;
}

// Close on outside click
window.onclick = (e) => { if (e.target == modal) closeModal(); };

// Keyboard support for 2026 accessibility
document.addEventListener('keydown', (e) => {
  if (modal.style.display === "block") {
    if (e.key === "ArrowLeft") changeImage(-1);
    if (e.key === "ArrowRight") changeImage(1);
    if (e.key === "Escape") closeModal();
  }
});