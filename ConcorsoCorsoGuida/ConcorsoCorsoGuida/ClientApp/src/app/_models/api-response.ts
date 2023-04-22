
export interface ApiResponse<T>
{
  result: T;
  hasError: boolean;
  message: string;
}
