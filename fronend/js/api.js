// frontend/js/api.js

// Конфигурация - проверете на кой порт ви върви backend
const API_BASE_URL = 'https://localhost:5001/api'; // или 5000, 7000, 8000

// Помощна функция за вземане на токен
function getAuthToken() {
    return localStorage.getItem('token');
}

// Помощна функция за headers
function getHeaders(includeAuth = true) {
    const headers = {
        'Content-Type': 'application/json'
    };
    
    if (includeAuth) {
        const token = getAuthToken();
        if (token) {
            headers['Authorization'] = `Bearer ${token}`;
        }
    }
    
    return headers;
}

// API функции
const api = {
    // ========== АУТЕНТИКАЦИЯ ==========
    async register(userData) {
        try {
            const response = await fetch(`${API_BASE_URL}/auth/register`, {
                method: 'POST',
                headers: getHeaders(false),
                body: JSON.stringify(userData)
            });
            
            const data = await response.json();
            if (data.token) {
                localStorage.setItem('token', data.token);
                localStorage.setItem('user', JSON.stringify(data.user));
            }
            return data;
        } catch (error) {
            console.error('Register error:', error);
            return { success: false, message: 'Network error' };
        }
    },

    async login(email, password) {
        try {
            const response = await fetch(`${API_BASE_URL}/auth/login`, {
                method: 'POST',
                headers: getHeaders(false),
                body: JSON.stringify({ email, password })
            });
            
            const data = await response.json();
            if (data.token) {
                localStorage.setItem('token', data.token);
                localStorage.setItem('user', JSON.stringify(data.user));
            }
            return data;
        } catch (error) {
            console.error('Login error:', error);
            return { success: false, message: 'Network error' };
        }
    },

    logout() {
        localStorage.removeItem('token');
        localStorage.removeItem('user');
        window.location.href = 'sign in.html';
    },

    getCurrentUser() {
        const user = localStorage.getItem('user');
        return user ? JSON.parse(user) : null;
    },

    // ========== СЪБИТИЯ ==========
    async getMyEvents() {
        try {
            const response = await fetch(`${API_BASE_URL}/events/my-events`, {
                headers: getHeaders(true)
            });
            return await response.json();
        } catch (error) {
            console.error('Get events error:', error);
            return { success: false, data: [] };
        }
    },

    async createEvent(eventData) {
        try {
            const response = await fetch(`${API_BASE_URL}/events`, {
                method: 'POST',
                headers: getHeaders(true),
                body: JSON.stringify(eventData)
            });
            return await response.json();
        } catch (error) {
            console.error('Create event error:', error);
            return { success: false, message: 'Network error' };
        }
    },

    async generatePlan(eventType, vibe, guests, date) {
        try {
            const response = await fetch(
                `${API_BASE_URL}/events/generate-plan?eventType=${eventType}&vibe=${vibe}&guests=${guests}&date=${date}`,
                {
                    headers: getHeaders(true)
                }
            );
            return await response.json();
        } catch (error) {
            console.error('Generate plan error:', error);
            return { success: false };
        }
    },

    // ========== ТЕМПЛЕЙТИ ==========
    async getTemplates(category) {
        try {
            const url = category 
                ? `${API_BASE_URL}/templates?category=${category}`
                : `${API_BASE_URL}/templates`;
            
            const response = await fetch(url, {
                headers: getHeaders(false)
            });
            return await response.json();
        } catch (error) {
            console.error('Get templates error:', error);
            return { success: false, data: [] };
        }
    }
};

// Експорт за използване в други файлове
window.api = api;