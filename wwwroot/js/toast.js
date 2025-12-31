// Scripts/toast.js
// Dependency: FontAwesome (để hiện icon), và cần có <div id="toastContainer"></div>

window.Toast = (function () {
    const containerId = "toastContainer";
    const defaultDuration = 5000;

    const icons = {
        success: "fa-circle-check",
        error: "fa-circle-exclamation",
        info: "fa-circle-info",
        warning: "fa-triangle-exclamation"
    };

    function getContainer() {
        let el = document.getElementById(containerId);
        if (!el) {
            // fallback: auto-create container nếu quên render partial
            el = document.createElement("div");
            el.id = containerId;
            el.className = "toast-container";
            document.body.appendChild(el);
        }
        return el;
    }

    function createToast({ type = "info", title = "", message = "", duration = defaultDuration }) {
        const container = getContainer();
        const toast = document.createElement("div");
        toast.className = `custom-toast ${type}`;

        const iconClass = icons[type] || icons.info;

        toast.innerHTML = `
            <i class="fa-solid ${iconClass} custom-toast-icon"></i>
            <div class="custom-toast-content">
                ${title ? `<div class="custom-toast-title">${escapeHtml(title)}</div>` : ""}
                ${message ? `<div class="custom-toast-message">${escapeHtml(message)}</div>` : ""}
            </div>
            <button class="custom-toast-close" type="button" aria-label="Close">
                <i class="fa-solid fa-xmark"></i>
            </button>
        `;

        // Close handler
        toast.querySelector(".custom-toast-close").addEventListener("click", () => hideToast(toast));

        // Add to DOM
        container.appendChild(toast);

        // Trigger animation
        requestAnimationFrame(() => toast.classList.add("show"));

        // Auto hide
        let timeoutId = null;
        if (duration > 0) {
            timeoutId = setTimeout(() => hideToast(toast), duration);
        }

        // trả về handle để bạn điều khiển nếu cần
        return {
            element: toast,
            hide: () => {
                if (timeoutId) clearTimeout(timeoutId);
                hideToast(toast);
            }
        };
    }

    function hideToast(toastEl) {
        if (!toastEl) return;
        toastEl.classList.remove("show");
        // chờ transition xong rồi remove
        setTimeout(() => {
            if (toastEl && toastEl.parentNode) toastEl.parentNode.removeChild(toastEl);
        }, 300);
    }

    // Escape để tránh XSS khi message/title lấy từ server/user
    function escapeHtml(str) {
        return String(str)
            .replace(/&/g, "&amp;")
            .replace(/</g, "&lt;")
            .replace(/>/g, "&gt;")
            .replace(/"/g, "&quot;")
            .replace(/'/g, "&#039;");
    }

    // Public API
    return {
        show: (title, message, options = {}) => createToast({ title, message, ...options }),
        success: (title, message, options = {}) => createToast({ type: "success", title, message, ...options }),
        error: (title, message, options = {}) => createToast({ type: "error", title, message, ...options }),
        info: (title, message, options = {}) => createToast({ type: "info", title, message, ...options }),
        warning: (title, message, options = {}) => createToast({ type: "warning", title, message, ...options }),
    };
})();
