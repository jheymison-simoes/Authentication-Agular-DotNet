export interface BaseResponse<T> {
  containError: boolean;
  messageError: string;
  response: T;
}
