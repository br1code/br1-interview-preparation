import { ZodSchema } from 'zod';

const API_URL =
  typeof window === 'undefined'
    ? process.env.API_URL // Server-side
    : process.env.NEXT_PUBLIC_API_URL; // Client-side

export async function fetchData<T>(
  url: string,
  schema: ZodSchema<T>
): Promise<T> {
  try {
    const response = await fetch(`${API_URL}/${url}`);

    if (!response.ok) {
      throw new Error('Failed to fetch data');
    }

    const data = await response.json();
    return schema.parse(data);
  } catch (error) {
    console.log(error);
    throw new Error(`Failed to fetch data: ${(error as Error).message}`);
  }
}

export async function postData<T, U>(
  url: string,
  schema: ZodSchema<T>,
  data: U | FormData
): Promise<T> {
  try {
    const isFormData = data instanceof FormData;

    const response = await fetch(`${API_URL}/${url}`, {
      method: 'POST',
      headers: isFormData ? undefined : { 'Content-Type': 'application/json' },
      body: isFormData ? data : JSON.stringify(data),
    });

    if (!response.ok) {
      throw new Error('Failed to submit data');
    }

    const responseData = await response.json();
    return schema.parse(responseData);
  } catch (error) {
    throw new Error(`Failed to submit data: ${(error as Error).message}`);
  }
}
