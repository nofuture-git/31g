import java.lang.*;
import java.lang.reflect.Constructor;
import java.lang.reflect.Field;
import java.lang.reflect.Method;
import java.lang.reflect.Member;
import java.io.*;

enum ClassMember { CONSTRUCTOR, FIELD, METHOD, ALL }

public class GetJavaMember {
    public static void main(String... args) {
		try {
			FileWriter fw = new FileWriter(args[0]);
			Class<?> c = Class.forName(args[1]);
			String className = c.getCanonicalName();
			fw.write(String.format("%s|%s|%s|%s%n","TypeName","Name","MemberType","Definition"));

			Package p = c.getPackage();
			String pkgName = (p != null ? p.getName() : "");

			for (int i = 2; i < args.length; i++) {
				switch (ClassMember.valueOf(args[i])) {
				case CONSTRUCTOR:
					printMembers(c.getConstructors(), "Constructor", className, fw);
					break;
				case FIELD:
					printMembers(c.getFields(), "Fields", className, fw);
					break;
				case METHOD:
					printMembers(c.getMethods(), "Methods", className, fw);
					break;
				case ALL:
					printMembers(c.getConstructors(), "Constuctors", className, fw);
					printMembers(c.getFields(), "Fields", className, fw);
					printMembers(c.getMethods(), "Methods", className, fw);
					break;
				default:
					assert false;
				}// end switch
			}//end for
			
			fw.flush();
			fw.close();

			// production code should handle these exceptions more gracefully
		} catch (ClassNotFoundException x) {
			//eat it
			
		} catch (java.io.IOException iox) {
		
		}
		
    }

    private static void printMembers(Member[] mbrs, String s,String className, FileWriter fw) throws java.io.IOException {
		for (Member mbr : mbrs) {
			if (mbr instanceof Field)
				fw.write(String.format("%s|%s|%s|%s%n",className,mbr.getName(),"Property",((Field)mbr).toGenericString()));
			else if (mbr instanceof Constructor)
				fw.write(String.format("%s|%s|%s|%s%n",className,mbr.getName(),"Method", ((Constructor)mbr).toGenericString()));
			else if (mbr instanceof Method)
				fw.write(String.format("%s|%s|%s|%s%n", className,mbr.getName(),"Method", ((Method)mbr).toGenericString()));
		}
    }

}