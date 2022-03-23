import React, { useEffect, useState } from "react";
import AdvancedModeSwitch from "./components/AdvancedModeSwitch";
import Release from "./entities/Release";
import ReleaseTable from "./components/ReleaseTable";
import { getAllReleasesAsync } from "./requests/releaseRequests";
import "./main.css";

const Application = (): JSX.Element => {
  const [releases, setReleases] = useState<Release[]>([]);
  const [releasesLoaded, setReleasesLoaded] = useState<boolean>(false);
  const [advancedModeEnabled, setAdvancedModeEnabled] = useState<boolean>(false);

  useEffect(() => {
    if (!releasesLoaded) {
      getAllReleasesAsync().then((releases) => {
        setReleases(releases);
        setReleasesLoaded(true);
      });
    }
  }, [releases]);

  return (
    <>
      <h1>Anime Library Browser</h1>
      <ReleaseTable releases={releases} advancedModeEnabled={advancedModeEnabled} />
      <details>
        <summary>Advanced Mode</summary>
        <AdvancedModeSwitch advancedModeEnabled={advancedModeEnabled} advancedModeToggled={(enabled) => setAdvancedModeEnabled(enabled)} />
      </details>
      <p>Copyright @ 2022 Andrey Talanin, Leonid Yakhtin, Maxim Zarkov. Built with ASP.NET Core, TypeScript &#38; React.</p>
    </>
  );
};

export default Application;
