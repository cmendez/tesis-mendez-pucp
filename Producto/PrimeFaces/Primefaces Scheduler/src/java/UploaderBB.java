
import java.io.File;
import java.io.FileOutputStream;
import java.io.IOException;
import java.io.InputStream;
import java.io.OutputStream;
import javax.faces.application.FacesMessage;

import javax.faces.bean.ManagedBean;
import javax.faces.bean.SessionScoped;
import javax.faces.context.FacesContext;

import org.primefaces.event.FileUploadEvent;
import org.primefaces.model.DefaultStreamedContent;
import org.primefaces.model.StreamedContent;

@ManagedBean(name = "uploaderBB")
@SessionScoped
public class UploaderBB {

    public void handleFileUpload(FileUploadEvent event) {
        System.out.println("Titulo: ");
        try {
            FacesMessage message = new FacesMessage(FacesMessage.SEVERITY_INFO, "Número de eventos", "algo mas");
            addMessage(message);

            File targetFolder = new File("C:\\Users\\Christian\\Documents\\PUCP\\2013-1\\TESIS1\\Repositorio\\Producto\\Primefaces Scheduler\\web\\imagenes");
            //File targetFolder = new File("/var/uploaded/imagenes");
            InputStream inputStream = event.getFile().getInputstream();
            OutputStream out = new FileOutputStream(new File(targetFolder,
                    event.getFile().getFileName()));
            int read = 0;
            byte[] bytes = new byte[1024];

            while ((read = inputStream.read(bytes)) != -1) {
                out.write(bytes, 0, read);
            }
            inputStream.close();
            out.flush();
            out.close();
        } catch (IOException e) {
            e.printStackTrace();
        }
    }

    private void addMessage(FacesMessage message) {
        FacesContext.getCurrentInstance().addMessage(null, message);
    }
}