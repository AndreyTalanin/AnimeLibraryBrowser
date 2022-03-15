import axios from "axios";
import Release from "../entities/Release";

export async function getAllReleasesAsync(): Promise<Release[]> {
  try {
    const { data } = await axios.get<Release[]>("/api/Release");
    return data;
  } catch (error) {
    return Promise.reject(error);
  }
}

export async function getReleaseAsync(title: string): Promise<Release> {
  try {
    const { data } = await axios.get<Release>(`/api/Release/${encodeURI(title)}`);
    return data;
  } catch (error) {
    return Promise.reject(error);
  }
}
