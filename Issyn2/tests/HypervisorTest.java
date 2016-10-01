import static org.junit.Assert.assertEquals;
import static org.junit.Assert.assertTrue;

import com.chmr.Hypervisor;
import org.junit.Test;

import java.net.MalformedURLException;
import java.net.URL;

public class HypervisorTest {
    @Test
    public void testRun() throws MalformedURLException, IllegalAccessException, InstantiationException, ClassNotFoundException {
        Hypervisor h = new Hypervisor(new URL("http://web.fritz.box/Kamera/"));
        assertEquals(h.Run(), true);
    }
    @Test
    public void testDownload() throws MalformedURLException {
        Hypervisor h = new Hypervisor(new URL("https://0fury.de"));
        String got = h.Download(new URL("https://0fury.de"));
        assertTrue(!got.equals(""));
    }
}