// Dynamic Toast Notification System
function showToast(message, type = 'success') {
    const container = document.getElementById('toastContainer');
    if (!container) return;

    const toast = document.createElement('div');
    toast.className = `toast-premium ${type} p-3 mb-2`;
    toast.style.opacity = '0';
    toast.style.transform = 'translateY(-20px)';
    toast.style.transition = 'all 0.3s cubic-bezier(0.175, 0.885, 0.32, 1.275)';

    let icon = 'fa-circle-check text-success';
    if (type === 'danger') icon = 'fa-triangle-exclamation text-danger';
    if (type === 'warning') icon = 'fa-circle-exclamation text-warning';

    toast.innerHTML = `
        <div class="d-flex align-items-center justify-content-between">
            <div class="d-flex align-items-center gap-2">
                <i class="fa-solid ${icon} fs-5"></i>
                <div class="small fw-semibold">${message}</div>
            </div>
            <button type="button" class="btn-close btn-close-white ms-3" style="font-size: 0.75rem;" onclick="this.parentElement.parentElement.remove()"></button>
        </div>
    `;

    container.appendChild(toast);

    // Trigger animation
    setTimeout(() => {
        toast.style.opacity = '1';
        toast.style.transform = 'translateY(0)';
    }, 10);

    // Auto remove
    setTimeout(() => {
        toast.style.opacity = '0';
        toast.style.transform = 'translateX(100px)';
        setTimeout(() => toast.remove(), 300);
    }, 4500);
}
