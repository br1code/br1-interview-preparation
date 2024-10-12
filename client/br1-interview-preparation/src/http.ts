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
