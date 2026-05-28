const API_BASE_URL = import.meta.env.VITE_API_BASE_URL ?? 'http://localhost:5227/api';

async function parseResponse(response) {
  const text = await response.text();

  if (!text) {
    return null;
  }

  try {
    return JSON.parse(text);
  } catch {
    return text;
  }
}

function getErrorMessage(payload, fallback) {
  if (!payload) {
    return fallback;
  }

  if (typeof payload === 'string') {
    return payload;
  }

  return payload.mesaj ?? payload.message ?? payload.title ?? fallback;
}

async function request(path, options = {}) {
  const response = await fetch(`${API_BASE_URL}/${path}`, {
    headers: {
      'Content-Type': 'application/json',
      ...options.headers
    },
    ...options
  });

  const payload = await parseResponse(response);

  if (!response.ok) {
    throw new Error(getErrorMessage(payload, 'Islem tamamlanamadi.'));
  }

  return payload;
}

export const apiClient = {
  list(resource) {
    return request(resource);
  },
  create(resource, data) {
    return request(resource, {
      method: 'POST',
      body: JSON.stringify(data)
    });
  },
  update(resource, id, data) {
    return request(`${resource}/${id}`, {
      method: 'PUT',
      body: JSON.stringify(data)
    });
  },
  remove(resource, id) {
    return request(`${resource}/${id}`, {
      method: 'DELETE'
    });
  }
};
