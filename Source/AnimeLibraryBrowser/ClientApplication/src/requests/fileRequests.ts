import axios from "axios";
import fileDownload from "js-file-download";
import { parse } from "content-disposition-header";

export async function downloadFileAsync(relativePath: string, fileName: string): Promise<void> {
  try {
    const { data, headers } = await axios.get(`/api/File?relativePath=${relativePath}&fileName=${fileName}`, { responseType: "blob" });
    const { parameters } = parse(headers["content-disposition"]);
    fileDownload(data, parameters["filename"]);
  } catch (error) {
    return Promise.reject(error);
  }
}
