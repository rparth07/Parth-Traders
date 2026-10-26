export function apiErrorMessage(error: any, fallback = 'Unable to complete the request. Please try again.') : string {
  if (!error || error.status === 0 || error.status >= 500) return fallback;
  const body = error.error;
  if (typeof body === 'string') return body;
  if (typeof body?.message === 'string') return body.message;
  if (body?.errors) return Object.values(body.errors).flat().filter(value => typeof value === 'string').join(' ') || fallback;
  return fallback;
}
